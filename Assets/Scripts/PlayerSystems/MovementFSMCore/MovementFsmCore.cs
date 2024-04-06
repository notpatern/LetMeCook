using System;
using PlayerSystems.MovementFSMCore.DataClass;
using PlayerSystems.MovementFSMCore.MovementContext;
using PlayerSystems.MovementFSMCore.MovementState;
using UnityEngine;
using UnityEngine.Events;


namespace PlayerSystems.MovementFSMCore
{
    [Serializable]
    public class MovementFsmCore : IStamina
    {
        [Header("References")] [SerializeField]
        private LayerMask isGround;

        public GameEventScriptableObject onFovChange;
        public GameEventScriptableObject onTiltChange;
        public CameraData cameraData;

        private FsmState _currentState;
        public FsmWallRunData wallRunData;
        public FsmAirData airData;
        public FsmGroundData groundData;
        public FsmDashData dashData;
        public StaminaData staminaData;

        public Transform camera;

        [Header("Player")] 
        [HideInInspector] public Rigidbody rb;
        public Transform orientation;
        public float Stamina { get; set; }
        private UnityEvent<float> _onStaminaUpdate = new UnityEvent<float>();

        [HideInInspector] public bool jumpHeld;
        [HideInInspector] public bool canJump;
        [HideInInspector] public bool canDash;
        [HideInInspector] public bool canWallRun;
        private bool _jumpInput;
        private bool _dashInput;
    
        public Vector2 Input { private set; get; }

        public void Init(Rigidbody rb)
        {
            this.rb = rb;
            Stamina = staminaData.maxStamina;
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
            if (Stamina <= staminaData.maxStamina)
            {
                RegenerateStamina(staminaData.staminaToRegenerate);
            }
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
            if (!_currentState.context.canJump || (!Grounded() && !canJump && _currentState.GetType() != typeof(WallRunState)))
            {
                return;
            }

            if (!Grounded() && _currentState.GetType() != typeof(WallRunState) &&
                CanConsumeStamina(staminaData.doubleJumpStamina))
            {
                ConsumeStamina(staminaData.doubleJumpStamina);
            }
            
            _jumpInput = true;
        }

        public void OnDashInputEvent()
        {
            if (!canDash || !_currentState.context.canDash || !CanConsumeStamina(staminaData.dashStamina))
            {
                return;
            }

            ConsumeStamina(staminaData.dashStamina);

            _dashInput = true;
        }

        public bool CanConsumeStamina(float staminaToConsume)
        {
            return !(Stamina - staminaToConsume < 0);
        }

        public void ConsumeStamina(float staminaToConsume)
        {
            if (Stamina - staminaToConsume <= 0)
            {
                Stamina = 0;
            }

            Stamina -= staminaToConsume;
            _onStaminaUpdate.Invoke(Stamina / staminaData.maxStamina);
        }

        public void BindStaminaRegeneration(UnityAction<float> action)
        {
            _onStaminaUpdate.AddListener(action);
        }

        public void RegenerateStamina(float staminaToRegenerate)
        {
            if(Stamina > staminaData.maxStamina)
            {
                return;
            }

            Stamina += staminaToRegenerate * Time.deltaTime;

            _onStaminaUpdate.Invoke(Stamina / staminaData.maxStamina);
        }

        public void ClearStamina()
        {
            Stamina -= staminaData.maxStamina;
        }
    }
}

