using PlayerSystems.MovementFSMCore.DataClass;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementContext
{
    public abstract class FsmContext
    {
        private readonly FsmData _fsmData;
        public float movementSpeed;
        public float drag;
        public float movementMultiplier;
        public float jumpForce;
        public float maxMovementSpeed;
        public bool useGravity;
        public bool canJump;
        public bool canDash;

        protected FsmContext(FsmData fsmData, bool canJump, bool canDash)
        {
            this._fsmData = fsmData;
            this.canJump = canJump;
            this.canDash = canDash;
        }

        public virtual void Init()
        {
            movementSpeed = _fsmData.movementSpeed;
            drag = _fsmData.drag;
            movementMultiplier = _fsmData.movementMultiplier;
            jumpForce = _fsmData.jumpForce;
            maxMovementSpeed = _fsmData.maxMovementSpeed;
            useGravity = _fsmData.useGravity;
        }
    }
}