using PlayerSystems.MovementFSMCore.DataClass;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class GroundContext : FsmContext
    {
        private readonly FsmGroundData _groundDataContext;

        public float friction;
        
        public GroundContext(FsmGroundData groundDataContext, bool canJump = true, bool canDash = false, bool canWallRun = true) : base(groundDataContext, canJump, canDash, canWallRun)
        {
            _groundDataContext = groundDataContext;
        }

        public override void Init()
        {
            base.Init();
            friction = _groundDataContext.friction;
        }
    }
}