using PlayerSystems.MovementFSMCore.DataClass;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class AirContext : FsmContext
    {
        private readonly FsmAirData _airDataContext;
        public float wallRunExitTime;
        
        public AirContext(FsmAirData airDataContext, float wallRunExitTime = 0) : base(airDataContext)
        {
            this._airDataContext = airDataContext;
            this.wallRunExitTime = wallRunExitTime;
        }
    }
}