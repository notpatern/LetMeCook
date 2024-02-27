using PlayerSystems.MovementFSMCore.MovementContext;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class AirState : FsmState
    {
        [Header("Wall Run Check")] 
        private LayerMask _isWall;
        private float _wallCheckDistance;
        private bool _wallRight;
        private bool _wallLeft;
        private RaycastHit _wallRightHit;
        private RaycastHit _wallLeftHit;
        private RaycastHit _wallHit;
        
        public AirState(FsmContext airState, MovementFsmCore fsmCore) : base(airState, fsmCore)
        {
            
        }
        public override void Init()
        {
            base.Init();
            _isWall = LayerMask.GetMask("isWall");
        }

        public override void Update()
        {
            base.Update();
            CheckForWall();
            if (!CanWallRun())
            {
                return;
            }
            fsmCore.SwitchState<WallRunState>(typeof(WallRunState), new WallRunContext(fsmCore.wallRunData));
        }
        
        private bool CanWallRun()
        {
            if (_wallLeft && movementDir == new Vector3(-1, 0, 1))
            {
                return true;
            }

            return _wallRight && movementDir == new Vector3(1,0, 1);
        }

        private void CheckForWall()
        {
            var position = fsmCore.transform.position;
            var right = fsmCore.orientation.right;
            
            _wallLeft = Physics.Raycast(
                position,
                -right,
                out _wallLeftHit,
                _wallCheckDistance,
                _isWall
            );
            
            _wallRight = Physics.Raycast(
                position,
                -right,
                out _wallRightHit,
                _wallCheckDistance,
                _isWall
            );
        }
    }
}