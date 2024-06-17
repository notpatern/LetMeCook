
using Audio;
using FMOD.Studio;
using FMODUnity;
using PlayerSystems.MovementFSMCore.MovementContext;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class WallRunState : FsmState
    {
        private readonly WallRunContext _context;
        private Vector3 _wallDirection;
        private Vector3 _wallNormal;

        EventInstance wallRunSoundInstance;

        public WallRunState(WallRunContext wallRunContext, MovementFsmCore fsmCore) : base(wallRunContext, fsmCore)
        {
            this._context = wallRunContext;
            wallRunSoundInstance = AudioManager.s_Instance.CreateInstance(AudioManager.s_Instance.m_AudioSoundData.m_PlayerWallrun);
            RuntimeManager.AttachInstanceToGameObject(wallRunSoundInstance, fsmCore.rb.transform);
        }

        public override void Init()
        {
            base.Init();
            _context.Init();
            _context.canJump = true;
            GetWallDirection();

            var velocity = fsmCore.rb.velocity;
            velocity = new Vector3(velocity.x, 0f, velocity.z);
            _context.maxMovementSpeed = velocity.magnitude;

            fsmCore.rb.velocity = _wallDirection * _context.maxMovementSpeed;

            wallRunSoundInstance.start();

            float[] fovValues = { fsmCore.cameraData.wallRunFov, fsmCore.cameraData.wallRunFovTimeToSet };
            fsmCore.onFovChange.TriggerEvent(fovValues);

            if (_context.wallRight)
            {
                float[] zTilt = { 0, fsmCore.cameraData.wallRunTilt, fsmCore.cameraData.wallRunTiltTimeToSet };
                fsmCore.onTiltChange.TriggerEvent(zTilt);
            }
            else
            {
                float[] zTilt = { 0, -fsmCore.cameraData.wallRunTilt, fsmCore.cameraData.wallRunTiltTimeToSet };
                fsmCore.onTiltChange.TriggerEvent(zTilt);
            }
        }

        private void GetWallDirection()
        {
            _wallNormal = _context.wallInfo.normal;
            _wallDirection = Vector3.Cross(_wallNormal, _context.wallInfo.transform.up);
            
            if ((fsmCore.orientation.forward - _wallDirection).magnitude > (fsmCore.orientation.forward + _wallDirection).magnitude)
            {
                _wallDirection = -_wallDirection;
            }

            Debug.Log(_wallDirection);
        }

        public override void Update()
        {
            WallTimer();
            fsmCore.ConsumeStamina(fsmCore.staminaData.wallRunStamina * Time.deltaTime);
        }

        public override void FixedUpdate()
        {
            if (CanStillWallRun() && fsmCore.Stamina > 0)
            {
                WallRunMovement();
                return;
            }
            ExitState();
        }

        private void WallTimer()
        {
            _context.wallTime -= Time.deltaTime;
        }

        private void WallRunMovement()
        {
            if (fsmCore.rb.velocity.magnitude > _context.maxMovementSpeed)
            {
                var velocity = fsmCore.rb.velocity.normalized * _context.maxMovementSpeed;
                fsmCore.rb.velocity = velocity;
            }
            fsmCore.rb.AddForce(_wallDirection * (context.movementSpeed * context.movementMultiplier), ForceMode.Force);
        }

        private bool CanStillWallRun()
        {
            return !(_context.wallTime <= 0) && (fsmCore.Input.x != 0 || fsmCore.Input.y > 0) && Physics.Raycast(fsmCore.rb.transform.position, -_wallNormal, 1.5f, LayerMask.GetMask("isWall"));
        }

        public override void Jump()
        {
            base.Jump();
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_PlayerDoubleJump);
            fsmCore.rb.AddForce(_wallNormal * _context.sideJumpForce + _context.wallInfo.transform.up * _context.upwardsJumpForce, ForceMode.Impulse);
            ExitState();
        }

        private void ExitState()
        {
            wallRunSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            float[] fovValues = { fsmCore.cameraData.defaultFov, fsmCore.cameraData.defaultFovTimeToSet };
            fsmCore.onFovChange.TriggerEvent(fovValues);
            float[] zTilt = { 0, fsmCore.cameraData.defaultTilt, fsmCore.cameraData.wallRunTiltTimeToSet };
            fsmCore.onTiltChange.TriggerEvent(zTilt);
            fsmCore.SwitchState<AirState>(typeof(AirState), new AirContext(fsmCore.airData, _context.exitTime, fsmCore.canJump, true, fsmCore.canWallRun, _context.wallInfo));
        }
    }
}