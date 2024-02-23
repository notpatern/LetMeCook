namespace PlayerSystems.MovementFSMCore
{
    public abstract class FsmState
    {
        public FsmContext context;
        protected FsmState(FsmContext context)
        {
            this.context = context;
        }

        public abstract void Init();
        public abstract void Update();
    }
}