using PlayerSystems.MovementFSMCore.MovementContext;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class FsmState
    {
        public readonly FsmContext context;
        protected readonly MovementFsmCore fsmCore;
        public bool jumpLeniency;
        protected Vector3 movementDir;
        

        protected FsmState(FsmContext context, MovementFsmCore fsmCore)
        {
            this.context = context;
            this.fsmCore = fsmCore;
        }

        public virtual void Init()
        {
            context.Init();
            fsmCore.rb.drag = context.drag;
            fsmCore.rb.useGravity = context.useGravity;
        }

        public virtual void Update()
        {
            jumpLeniency = IsGroundUnderForJumpLeniency();
        }

        public void DecreaseCoyoteTime()
        {
            fsmCore.coyoteTime -= Time.deltaTime;
        }

        public virtual void FixedUpdate()
        {
            Movement();
        }

        protected virtual Vector2 FindVelRelativeToLook()
        {
            float lookAngle = fsmCore.orientation.transform.eulerAngles.y;
            var velocity = fsmCore.rb.velocity;
            float moveAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

            float u = Mathf.DeltaAngle(lookAngle, moveAngle);
            float v = 90 - u;

            float magnitude = fsmCore.rb.velocity.magnitude;
            float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
            float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

            return new Vector2(xMag, yMag);
        }

        protected virtual void Movement()
        {
            Vector2 mag = FindVelRelativeToLook();
            float xMag = mag.x, yMag = mag.y;

            Vector2 newInput = fsmCore.Input;

            if (newInput.x > 0 && xMag > context.maxMovementSpeed) newInput.x = 0;
            if (newInput.x < 0 && xMag < -context.maxMovementSpeed) newInput.x = 0;
            if (newInput.y > 0 && yMag > context.maxMovementSpeed) newInput.y = 0;
            if (newInput.y < 0 && yMag < -context.maxMovementSpeed) newInput.y = 0;

            movementDir = fsmCore.orientation.forward * newInput.y +
                          fsmCore.orientation.right * newInput.x;

            fsmCore.rb.AddForce(movementDir.normalized * (context.movementSpeed * context.movementMultiplier), ForceMode.Force);
        }

        public virtual void Jump()
        {
            var vel = fsmCore.rb.velocity;
            fsmCore.rb.velocity = new Vector3(vel.x, 0, vel.z);

            fsmCore.rb.AddForce(Vector3.up * context.jumpForce, ForceMode.Impulse);
        }

        public bool IsGroundUnderForJumpLeniency()
        {
            return Physics.Raycast(fsmCore.rb.position, Vector3.down, context.jumpLeniency, fsmCore.isGround);
        }

        public virtual void Dash()
        {
            var vel = fsmCore.rb.velocity;
            fsmCore.rb.velocity = new Vector3();
            context.canDash = false;
            fsmCore.SwitchState<DashState>(typeof(DashState), new DashContext(fsmCore.dashData, context.canJump, context.canDash));
        }
    }
}