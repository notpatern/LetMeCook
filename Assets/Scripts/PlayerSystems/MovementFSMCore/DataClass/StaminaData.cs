using UnityEngine;

namespace PlayerSystems.MovementFSMCore.DataClass
{
    [CreateAssetMenu(menuName = "LetMeCook/MovementData/StaminaData")]
    public class StaminaData : ScriptableObject
    {
        public float maxStamina;
        public float staminaToRegenerate;
        public float dashStamina;
        public float doubleJumpStamina;
        public float wallRunStamina;
    }
}