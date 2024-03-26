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
        public bool canWallRun;

        protected FsmContext(FsmData fsmData, bool canJump, bool canDash, bool canWallRun)
        {
            this._fsmData = fsmData;
            this.canJump = canJump;
            this.canDash = canDash;
            this.canWallRun = canWallRun;
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