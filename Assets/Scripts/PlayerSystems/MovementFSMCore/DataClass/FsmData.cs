using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    public class FsmData : ScriptableObject
    {
        public float movementSpeed;
        public float drag;
        public float movementMultiplier;
        public float jumpForce;
    }
}