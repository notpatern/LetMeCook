using PlayerSystems.MovementFSMCore.DataClass;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class GroundContext: FsmContext
    {
        private readonly FsmGroundData _groundDataContext;
        
        public GroundContext(FsmGroundData groundDataContext) : base(groundDataContext)
        {
        }
    }
}