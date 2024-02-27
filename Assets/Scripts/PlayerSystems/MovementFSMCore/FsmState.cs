using UnityEngine;
using UnityEngine.PlayerLoop;

namespace PlayerSystems.MovementFSMCore
{
    public abstract class FsmState
    {
        protected readonly FsmContext context;
        protected readonly MovementFsmCore fsmCore;
        public bool canJump;
        public Vector3 movementDir;
        private Vector2 input;
        
        protected FsmState(FsmContext context, MovementFsmCore fsmCore)
        {
            this.context = context;
            this.fsmCore = fsmCore;
        }

        public virtual void Init()
        {
            context.Init();
            fsmCore.rb.drag = context.drag;
        }

        public virtual void Update()
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

        public void UpdateMovementInput(Vector2 newInput)
        {
            input = newInput;
        }
        
        protected virtual void Movement()
        {
            Vector2 mag = FindVelRelativeToLook();
            float xMag = mag.x, yMag = mag.y;

            if (input.x > 0 && xMag > context.maxMovementSpeed) input.x = 0;
            if (input.x < 0 && xMag < -context.maxMovementSpeed) input.x = 0;
            if (input.y > 0 && yMag > context.maxMovementSpeed) input.y = 0;
            if (input.y < 0 && yMag < -context.maxMovementSpeed) input.y = 0;
            
            movementDir = fsmCore.orientation.forward * input.y +
                           fsmCore.orientation.right * input.x;
            
            fsmCore.rb.AddForce(movementDir * (context.movementSpeed * context.movementMultiplier), ForceMode.Force);
        }

        public virtual void Jump()
        {
            fsmCore.rb.AddForce(Vector3.up * context.jumpForce);
        }
    }
}