
using PlayerSystems.MovementFSMCore.DataClass;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class WallRunContext : FsmContext
    {
        private readonly FsmWallRunData _wallRunContext;
        public float sideJumpForce;
        public float exitTime;
        public float wallGravity;
        public float wallCheckDistance;
        
        public WallRunContext(FsmWallRunData wallRunContext) : base(wallRunContext)
        {
            this._wallRunContext = wallRunContext;
        }

        protected override void Init()
        {
            base.Init();
            sideJumpForce = _wallRunContext.sideJumpForce;
            exitTime = _wallRunContext.exitTime;
            wallGravity = _wallRunContext.wallGravity;
            wallCheckDistance = _wallRunContext.wallCheckDistance;
        }
    }
}