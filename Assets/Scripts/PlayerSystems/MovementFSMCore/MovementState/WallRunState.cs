
using PlayerSystems.MovementFSMCore.MovementContext;
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
            _context.maxMovementSpeed = fsmCore.rb.velocity.magnitude + _context.maxMovementSpeed;
            _context.canJump = true;
            GetWallDirection();
            var velocity = fsmCore.rb.velocity;
            velocity = new Vector3(velocity.x, 0f, velocity.z);
            fsmCore.rb.velocity = velocity;
        }

        private void GetWallDirection()
        {
            _wallNormal = _context.wallInfo.normal;
            _wallDirection = Vector3.Cross(_wallNormal, fsmCore.rb.transform.up);
            
            if ((fsmCore.orientation.forward - _wallDirection).magnitude > (fsmCore.orientation.forward + _wallDirection).magnitude && fsmCore.Input.y > 0)
            {
                _wallDirection = -_wallDirection;
            }
        }

        public override void Update()
        {
            WallTimer();
        }

        public override void FixedUpdate()
        {
            if (CanStillWallRun())
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
                var velocity = fsmCore.rb.velocity.normalized * (context.maxMovementSpeed * context.movementMultiplier);
                fsmCore.rb.velocity = velocity;
            }
            fsmCore.rb.AddForce(_wallDirection * (context.movementSpeed * context.movementMultiplier), ForceMode.Force);
            fsmCore.rb.AddForce(Vector3.down * _context.wallGravity, ForceMode.Force);
        }

        private bool CanStillWallRun()
        {
            return !(_context.wallTime <= 0) && fsmCore.Input.y != 0 && Physics.Raycast(fsmCore.rb.transform.position, -_wallNormal, 1f, LayerMask.GetMask("isWall"));
        }

        public override void Jump()
        {
            base.Jump();
            fsmCore.rb.AddForce(_wallNormal * _context.sideJumpForce, ForceMode.Impulse);
            ExitState();
        }

        private void ExitState()
        {
            fsmCore.SwitchState<AirState>(typeof(AirState), new AirContext(fsmCore.airData, _context.exitTime, _context.canJump, true, _context.wallInfo));
        }
    }
}