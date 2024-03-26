
using PlayerSystems.MovementFSMCore.DataClass;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public class WallRunContext : FsmContext
    {
        private readonly FsmWallRunData _wallRunContext;
        public float sideJumpForce;
        public float exitTime;
        public float wallGravity;
        public float wallTime;
        public RaycastHit wallInfo;
        
        public WallRunContext(FsmWallRunData wallRunContext, RaycastHit wallInfo, bool canJump = true, bool canDash = false, bool canWallRun = true) : base(wallRunContext, canJump, canDash, canWallRun)
        {
            this._wallRunContext = wallRunContext;
            this.wallInfo = wallInfo;
        }

        public override void Init()
        {
            base.Init();
            sideJumpForce = _wallRunContext.sideJumpForce;
            exitTime = _wallRunContext.exitTime;
            wallGravity = _wallRunContext.wallGravity;
            wallTime = _wallRunContext.wallTime;
        }
    }
}