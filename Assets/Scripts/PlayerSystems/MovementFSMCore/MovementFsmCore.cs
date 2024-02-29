using System;
using Player.Input;
using PlayerSystems.MovementFSMCore.DataClass;
using PlayerSystems.MovementFSMCore.MovementContext;
using PlayerSystems.MovementFSMCore.MovementState;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;


namespace PlayerSystems.MovementFSMCore
{
    public class MovementFsmCore : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private LayerMask isGround;

        private FsmState _currentState;
        public FsmWallRunData wallRunData;
        public FsmAirData airData;
        public FsmGroundData groundData;
        public FsmDashData dashData;

        public new Transform camera;

        [Header("Player")] public Rigidbody rb;
        public Transform orientation;

        public Vector2 Input { private set; get; }

        private void Start()
        {
            _currentState = new GroundState(new GroundContext(groundData), this);
            _currentState.Init();
        }
    
        private void Update()
        {
            _currentState.Update();
            HandleGroundedState();
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdate();
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
            if (Grounded() && _currentState.GetType() == typeof(AirState))
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
            if (!_currentState.context.canJump)
            {
                return;
            }
            
            _currentState.Jump();
        }

        public void OnDashInputEvent()
        {
            if (!_currentState.context.canDash)
            {
                return;
            }
            
            _currentState.Dash();
        }
    }
}

