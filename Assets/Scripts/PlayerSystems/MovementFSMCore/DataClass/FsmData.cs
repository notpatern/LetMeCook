using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    public class FsmData : ScriptableObject
    {
        public float jumpLeniency;
        public float movementSpeed;
        public float drag;
        public float movementMultiplier;
        public float jumpForce;
        public float maxMovementSpeed;
        public bool useGravity;
    }
}