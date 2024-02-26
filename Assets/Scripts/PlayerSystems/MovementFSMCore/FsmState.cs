namespace PlayerSystems.MovementFSMCore
{
    public abstract class FsmState
    {
        public FsmContext context;
        public MovementFsmCore fsmCore;
        public bool canJump;
        
        protected FsmState(FsmContext context, MovementFsmCore fsmCore)
        {
            this.context = context;
            this.fsmCore = fsmCore;
        }

        public abstract void Init();
        public abstract void Update();
    }
}