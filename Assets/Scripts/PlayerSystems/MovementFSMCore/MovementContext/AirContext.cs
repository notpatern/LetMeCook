using PlayerSystems.MovementFSMCore.DataClass;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class AirContext : FsmContext
    {
        private readonly FsmAirData _airDataContext;
        public float wallRunExitTime;
        public float wallCheckDistance;
        
        public AirContext(FsmAirData airDataContext, float wallRunExitTime = 0, bool canJump = true, bool canDash = true) : base(airDataContext, canJump, canDash)
        {
            this._airDataContext = airDataContext;
            this.wallRunExitTime = wallRunExitTime;
        }

        public override void Init()
        {
            base.Init();
            wallCheckDistance = _airDataContext.wallCheckDistance;
        }
    }
}