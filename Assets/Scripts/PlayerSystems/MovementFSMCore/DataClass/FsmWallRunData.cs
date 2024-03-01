using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    [CreateAssetMenu(menuName = "LetMeCook/MovementData/WallRunData")]
    public class FsmWallRunData : FsmData
    {
        public float sideJumpForce;
        public float exitTime;
        public float wallGravity;
        public float wallTime;
    }
}