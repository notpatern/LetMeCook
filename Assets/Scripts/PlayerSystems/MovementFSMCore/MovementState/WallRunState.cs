
using PlayerSystems.MovementFSMCore.MovementContext;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class WallRunState : FsmState
    {
        private readonly WallRunContext _context;
        private Vector3 _wallDirection;
        private Vector3 _wallNormal;
        public WallRunState(WallRunContext wallRunContext, MovementFsmCore fsmCore) : base(wallRunContext, fsmCore)
        {
            this._context = wallRunContext;
        }

        public override void Init()
        {
            base.Init();
            _context.Init();
            _context.canJump = true;
            GetWallDirection();

            fsmCore.wallRunSound.start();

            float[] fovValues = { fsmCore.cameraData.wallRunFov, fsmCore.cameraData.wallRunFovTimeToSet };
            fsmCore.onFovChange.TriggerEvent(fovValues);

            if (_context.wallRight)
            {
                float[] zTilt = { fsmCore.cameraData.wallRunTilt, fsmCore.cameraData.wallRunTiltTimeToSet };
                fsmCore.onTiltChange.TriggerEvent(zTilt);
            }
            else
            {
                float[] zTilt = { -fsmCore.cameraData.wallRunTilt, fsmCore.cameraData.wallRunTiltTimeToSet };
                fsmCore.onTiltChange.TriggerEvent(zTilt);
            }
            var velocity = fsmCore.rb.velocity;
            velocity = new Vector3(velocity.x, 0f, velocity.z);

            if (velocity.magnitude < _context.maxMovementSpeed)
            {
                velocity = velocity.normalized * _context.maxMovementSpeed;
            }

            fsmCore.rb.velocity = velocity;

            _context.maxMovementSpeed = fsmCore.rb.velocity.magnitude;
        }

        private void GetWallDirection()
        {
            _wallNormal = _context.wallInfo.normal;
            _wallDirection = Vector3.Cross(_wallNormal, fsmCore.rb.transform.up);
            
            if ((fsmCore.orientation.forward - _wallDirection).magnitude > (fsmCore.orientation.forward + _wallDirection).magnitude)
            {
                _wallDirection = -_wallDirection;
            }
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
            fsmCore.rb.AddForce(Vector3.down * _context.wallGravity, ForceMode.Force);
        }

        private bool CanStillWallRun()
        {
            return !(_context.wallTime <= 0) && (fsmCore.Input.x != 0 || fsmCore.Input.y > 0) && Physics.Raycast(fsmCore.rb.transform.position, -_wallNormal, 1.5f, LayerMask.GetMask("isWall"));
        }

        public override void Jump()
        {
            base.Jump();
            fsmCore.rb.AddForce(_wallNormal * _context.sideJumpForce, ForceMode.Impulse);
            ExitState();
        }

        private void ExitState()
        {
            fsmCore.wallRunSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            float[] fovValues = { fsmCore.cameraData.defaultFov, fsmCore.cameraData.defaultFovTimeToSet };
            fsmCore.onFovChange.TriggerEvent(fovValues);
            float[] zTilt = { fsmCore.cameraData.defaultTilt, fsmCore.cameraData.wallRunTiltTimeToSet };
            fsmCore.onTiltChange.TriggerEvent(zTilt);
            fsmCore.SwitchState<AirState>(typeof(AirState), new AirContext(fsmCore.airData, _context.exitTime, fsmCore.canJump, true, fsmCore.canWallRun, _context.wallInfo));
        }
    }
}