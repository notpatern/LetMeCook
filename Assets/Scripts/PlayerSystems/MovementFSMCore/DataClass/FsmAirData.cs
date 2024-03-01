using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    [CreateAssetMenu(menuName = "LetMeCook/MovementData/AirData")]
    public class FsmAirData : FsmData
    {
        public float wallCheckDistance;
    }
}