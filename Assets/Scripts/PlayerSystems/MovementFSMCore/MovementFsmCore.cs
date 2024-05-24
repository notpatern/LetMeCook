using System;
using Audio;
using FMOD.Studio;
using PlayerSystems.MovementFSMCore.DataClass;
using PlayerSystems.MovementFSMCore.MovementContext;
using PlayerSystems.MovementFSMCore.MovementState;
using PostProcessing;
using UnityEngine;
using UnityEngine.Events;


namespace PlayerSystems.MovementFSMCore
{
    [Serializable]
    public class MovementFsmCore : IStamina
    {
        [Header("References")] [SerializeField]
        public LayerMask isGround;

        public GameEventScriptableObject onFovChange;
        public GameEventScriptableObject onTiltChange;
        public CameraData cameraData;

        private FsmState _currentState;
        public FsmWallRunData wallRunData;
        public FsmAirData airData;
        public FsmGroundData groundData;
        public FsmDashData dashData;
        public StaminaData staminaData;

        public EventInstance wallRunSound;

        public Transform camera;

        private PostProcessingManager postProcessingManager;

        [Header("Player")] 
        [HideInInspector] public Rigidbody rb;
        public Transform orientation;
        public float Stamina { get; set; }
        private UnityEvent<float> _onStaminaUpdate = new UnityEvent<float>();

        [SerializeField] Transform _playerSpeedEffect;

        [HideInInspector] public bool jumpHeld;
        [HideInInspector] public bool canJump;
        [HideInInspector] public bool canDash;
        [HideInInspector] public bool canWallRun;
        [HideInInspector] public float coyoteTime;
        [SerializeField] float _minimunSpeedEffect;
        [SerializeField] float _minimunSpeedLineEffect;
        [SerializeField] float _lineMinimumDuration = 0.5f;
        [SerializeField] float _lineMinimumMagnitudeDisparition = 10f;
        [SerializeField] float _lineSpeedEffectRotationSpeed = 3f;
        float _lineTimer = 0f;
        [SerializeField] float speedEffectMultiplier;
        private bool _jumpInput;
        private bool _dashInput;

        float _groundedTime = 0f;
    
        public Vector2 Input { private set; get; }
        [HideInInspector] public MonoBehaviour mono;

        public void Init(Rigidbody rb, GameEventScriptableObject ppm, MonoBehaviour mono)
        {
            this.mono = mono;
            this.rb = rb;
            ppm.BindEventAction(InitPostProcessingManger);
            Stamina = staminaData.maxStamina;
            canJump = false;
            canDash = false;
            canWallRun = false;
            _currentState = new GroundState(new GroundContext(groundData), this);
            _currentState.Init();
        }

        public void InitPostProcessingManger(object obj)
        {
            postProcessingManager = (PostProcessingManager)obj;
        }

        public void CreateSoundInstances() {
            wallRunSound = AudioManager.s_Instance.CreateInstance(AudioManager.s_Instance.m_AudioSoundData.m_PlayerWallrun);
        }

        public void Update()
        {
            HandleGroundedState();
            _currentState.Update();
            if (Stamina <= staminaData.maxStamina)
            {
                RegenerateStamina(staminaData.staminaToRegenerate);
            }

            if (_currentState.GetType() == typeof(GroundState))
            {
                _groundedTime += Time.deltaTime;
            }

            HandleLensDistortionBasedOnPlayerSpeed();
        }

        void HandleLensDistortionBasedOnPlayerSpeed()
        {
            if (rb.velocity.magnitude >= _minimunSpeedEffect)
            {
                float playerSpeed = _currentState.FindVelRelativeToLook().y;
                float value = playerSpeed - _minimunSpeedEffect;
                value = value * speedEffectMultiplier;

                postProcessingManager.ChangeMotionBlur(value);
            }

            Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (velocity.magnitude >= _minimunSpeedLineEffect || _lineTimer > 0f && velocity.magnitude > _lineMinimumMagnitudeDisparition)
            {
                if (!_playerSpeedEffect.gameObject.activeSelf)
                {
                    _lineTimer = _lineMinimumDuration;
                    _playerSpeedEffect.rotation = Quaternion.LookRotation(velocity);
                    _playerSpeedEffect.gameObject.SetActive(true);
                }

                _lineTimer -= Time.deltaTime;
                _playerSpeedEffect.rotation = Quaternion.Lerp(_playerSpeedEffect.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * _lineSpeedEffectRotationSpeed);
            }
            else if (_playerSpeedEffect.gameObject.activeSelf)
            {
                _lineTimer -= Time.deltaTime;

                if (_lineTimer <= 0f)
                {
                    _playerSpeedEffect.gameObject.SetActive(false);
                }
            }

        }

        public float GetGroundedTime()
        {
            return _groundedTime;
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
                coyoteTime = _currentState.context.coyoteTime;
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
            if ((!_currentState.context.canJump || (!Grounded() && !canJump && _currentState.GetType() != typeof(WallRunState))) && coyoteTime <= 0)
            {
                return;
            }

            if (!Grounded() && _currentState.GetType() != typeof(WallRunState) &&
                CanConsumeStamina(staminaData.doubleJumpStamina) && !_currentState.jumpLeniency && coyoteTime <= 0)
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

