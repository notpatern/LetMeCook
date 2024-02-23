using UnityEngine;

namespace PlayerSystems.MovementFSMCore
{
    public abstract class FsmContext
    {
        public ScriptableObject scriptableObject;
        public float movementSpeed;
        public FsmContext(ScriptableObject scriptableObject)
        {
            this.scriptableObject = scriptableObject;
        }
    }
}