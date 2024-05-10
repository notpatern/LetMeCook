
using PlayerSystems.MovementFSMCore.MovementContext;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class GroundState : FsmState
    {
        private readonly GroundContext _context;

        public GroundState(GroundContext context, MovementFsmCore fsmCore) : base(context, fsmCore)
        {
            _context = context;
        }

        public override void Update()
        {
            base.Update();
            FrictionManager();
        }

        private void FrictionManager()
        {
            if (fsmCore.jumpHeld)
            {
                fsmCore.rb.drag = 0f;
                return;
            }

            if (fsmCore.Input == new Vector2 (0, 0))
            {
                fsmCore.rb.drag = _context.friction;
            }
            else
            {
                fsmCore.rb.drag = context.drag;
            }
        }

        public override void Jump()
        {
            base.Jump();
            fsmCore.coyoteTime = 0f;
        }
    }
}