using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    [CreateAssetMenu(menuName = "LetMeCook/MovementData/AirData")]
    public class FsmAirData : FsmData
    {
        public float wallCheckDistance;
        public float doubleJumpForce;
        public float restartWallRunFalloffDistance;
        [Range(0f, 100f)]
        public float doubleJumpDeceleration;
    }
}