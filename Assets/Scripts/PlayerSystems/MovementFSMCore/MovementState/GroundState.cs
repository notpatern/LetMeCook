
using PlayerSystems.MovementFSMCore.MovementContext;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class GroundState : FsmState
    {
        public GroundState(GroundContext context, MovementFsmCore fsmCore) : base(context, fsmCore)
        {
        }

        public override void Jump()
        {
            base.Jump();
            fsmCore.coyoteTime = 0f;
        }
    }
}