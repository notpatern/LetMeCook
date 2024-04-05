namespace PlayerSystems.MovementFSMCore
{
    public interface IStamina
    {
        public float Stamina { get; set; }
        public bool ConsumeStamina(float staminaToRemove);
        public void RegenerateStamina(float staminaToRegenerate);
    }
}