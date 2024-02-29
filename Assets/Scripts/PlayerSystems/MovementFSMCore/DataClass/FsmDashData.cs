using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    [CreateAssetMenu(menuName = "LetMeCook/MovementData/DashData")]
    public class FsmDashData : FsmData
    {
        public float dashForce;
        public float dashDuration;
    }
}