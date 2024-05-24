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
        public float doubleJumpTilt;
        public float doubleJumpTiltTimeToSet;
        public float acceptedShakeDuration;
        public float rejectedShakeDuration;
        public float acceptedShakeStrength;
        public float rejectedShakeStrength;
        public float acceptedShakeVibrator;
        public float rejectedShakeVibrator;
        public float acceptedShakeRandomness;
        public float rejectedShakeRandomness;
    }
}