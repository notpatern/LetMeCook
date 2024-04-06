namespace PlayerSystems.MovementFSMCore
{
    public interface IStamina
    {
        public float Stamina { get; set; }
        public bool CanConsumeStamina(float staminaToRemove);
        public void ConsumeStamina(float staminaToRemove);
        public void ClearStamina();
        public void RegenerateStamina(float staminaToRegenerate);
    }
}