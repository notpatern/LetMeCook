using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    [CreateAssetMenu(menuName = "LetMeCook/CameraData")]
    public class CameraData : ScriptableObject
    {
        public float defaultFov;
        public float defaultFovTimeToSet;
        public float defaultTilt;
        public float dashFov;
        public float dashFovTimeToSet;
        public float wallRunFov;
        public float wallRunFovTimeToSet;
        public float wallRunTiltTimeToSet;
        public float wallRunTilt;
    }
}