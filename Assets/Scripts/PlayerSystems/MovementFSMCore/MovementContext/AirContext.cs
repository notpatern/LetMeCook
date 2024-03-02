using PlayerSystems.MovementFSMCore.DataClass;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class AirContext : FsmContext
    {
        private readonly FsmAirData _airDataContext;
        public float wallRunExitTime;
        public float wallCheckDistance;
        public RaycastHit previousWallInfo;
        public float restartWallRunFalloffDistance;
        
        public AirContext(FsmAirData airDataContext, float wallRunExitTime = 0, bool canJump = true, bool canDash = true, RaycastHit previousWallInfo = new RaycastHit()) : base(airDataContext, canJump, canDash)
        {
            this._airDataContext = airDataContext;
            this.wallRunExitTime = wallRunExitTime;
            this.previousWallInfo = previousWallInfo;
        }

        public override void Init()
        {
            base.Init();
            wallCheckDistance = _airDataContext.wallCheckDistance;
            restartWallRunFalloffDistance = _airDataContext.restartWallRunFalloffDistance;
        }
    }
}