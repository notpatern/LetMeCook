using System;
using PlayerSystems.MovementFSMCore.DataClass;
using PlayerSystems.MovementFSMCore.MovementContext;
using PlayerSystems.MovementFSMCore.MovementState;
using UnityEngine;


namespace PlayerSystems.MovementFSMCore
{
    [Serializable]
    public class MovementFsmCore
    {
        [Header("References")] [SerializeField]
        private LayerMask isGround;

        private FsmState _currentState;
        public FsmWallRunData wallRunData;
        public FsmAirData airData;
        public FsmGroundData groundData;
        public FsmDashData dashData;

        public Transform camera;

        [Header("Player")] 
        [HideInInspector] public Rigidbody rb;
        public Transform orientation;
        public bool jumpHeld;
        private bool _jumpInput;
        private bool _dashInput;
        public bool canJump;
        public bool canDash;
        public bool canWallRun;
    
        public Vector2 Input { private set; get; }

        public void Init(Rigidbody rb)
        {
            this.rb = rb;
            canJump = false;
            canDash = false;
            canWallRun = false;
            _currentState = new GroundState(new GroundContext(groundData), this);
            _currentState.Init();
        }
    
        public void Update()
        {
            HandleGroundedState();
            _currentState.Update();
        }

        public void UpdateWallRunState(bool state)
        {
            canWallRun = state;
        }

        public void UpdateDashState(bool state)
        {
            canDash = state;
        }

        public void UpdateDoubleJumpState(bool state)
        {
            canJump = state;
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
            
            if (jumpHeld && _currentState.GetType() == typeof(GroundState) || _jumpInput)
            {
                _currentState.Jump();
                _jumpInput = false;
            }
            if (_dashInput)
            {
                _currentState.Dash();
                _dashInput = false;
            }
        }

        public void SwitchState<TState>(Type state, FsmContext context) where TState : FsmState
        {
            _currentState = (TState)Activator.CreateInstance(
                state,
                context,
                this
            );
            _currentState.Init();
        }

        private void HandleGroundedState()
        {
            if (Grounded() && _currentState.GetType() != typeof(GroundState))
            {
                SwitchState<GroundState>(typeof(GroundState), new GroundContext(groundData, true, true));
            }
            else if (!Grounded() && _currentState.GetType() == typeof(GroundState))
            {
                SwitchState<AirState>(typeof(AirState), new AirContext(airData, 0f, _currentState.context.canJump, _currentState.context.canDash));
            }
        }

        private bool Grounded() 
        {
            return Physics.Raycast(
                rb.position,
                Vector3.down,
                1.1f,
                isGround
            );
        }
        
        public void OnMovementInputEvent(Vector2 input)
        {
            this.Input = input;
        }

        public void OnJumpInputEvent()
        {
            if (!_currentState.context.canJump || (!Grounded() && !canJump))
            {
                return;
            }
            
            _jumpInput = true;
        }

        public void OnDashInputEvent()
        {
            if (!canDash || !_currentState.context.canDash)
            {
                return;
            }

            _dashInput = true;
        }
    }
}

