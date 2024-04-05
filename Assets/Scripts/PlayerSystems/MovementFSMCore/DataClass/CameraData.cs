using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    [CreateAssetMenu(menuName = "LetMeCook/CameraData")]
    public class CameraData : ScriptableObject
    {
        public float defaultFov;
        public float defaultTilt;
        public float wallRunFov;
        public float wallRunTilt;
    }
}