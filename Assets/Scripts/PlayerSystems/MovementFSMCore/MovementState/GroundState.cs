
using PlayerSystems.MovementFSMCore.MovementContext;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class GroundState : FsmState
    {
        public GroundState(GroundContext context, MovementFsmCore fsmCore) : base(context, fsmCore)
        {
        }
    }
}