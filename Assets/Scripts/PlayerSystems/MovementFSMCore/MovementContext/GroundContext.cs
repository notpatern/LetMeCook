using PlayerSystems.MovementFSMCore.DataClass;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class GroundContext : FsmContext
    {
        private readonly FsmGroundData _groundDataContext;
        
        public GroundContext(FsmGroundData groundDataContext, bool canJump = true, bool canDash = false) : base(groundDataContext, canJump, canDash)
        {
        }
    }
}