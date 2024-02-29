using PlayerSystems.MovementFSMCore.DataClass;
using Unity.VisualScripting;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class DashContext : FsmContext
    {
        private readonly FsmDashData _dashContext;
        public float dashForce;
        public float dashDuration;
        
        public DashContext(FsmDashData dashContext, bool canJump = true, bool canDash = false) : base(dashContext, canJump, canDash)
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