using PlayerSystems.MovementFSMCore.DataClass;

namespace PlayerSystems.MovementFSMCore
{
    public abstract class FsmContext
    {
        private readonly FsmData _fsmData;
        public float movementSpeed;
        public float drag;
        public float movementMultiplier;
        public float jumpForce;

        protected FsmContext(FsmData fsmData)
        {
            this._fsmData = fsmData;
        }

        protected virtual void Init()
        {
            movementSpeed = _fsmData.movementSpeed;
            drag = _fsmData.drag;
            movementMultiplier = _fsmData.movementMultiplier;
            jumpForce = _fsmData.jumpForce;
        }
    }
}