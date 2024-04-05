using PlayerSystems.MovementFSMCore.DataClass;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class DashContext : FsmContext
    {
        private readonly FsmDashData _dashContext;
        public float dashForce;
        public float dashDuration;
        
        public DashContext(FsmDashData dashContext, bool canJump = true, bool canDash = false, bool canWallRun = true) : base(dashContext, canJump, canDash, canWallRun)
        {
            this._dashContext = dashContext;
        }

        public override void Init()
        {
            base.Init();
            dashForce = _dashContext.dashForce;
            dashDuration = _dashContext.dashDuration;
        }
    }
}