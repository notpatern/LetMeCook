using UnityEngine;
using FoodSystem.FoodMachinery.FoodTransformer;

namespace Tutorial
{
    public class TutorialBaker : Baker
    {
        [Header("Tuto"), SerializeField] TutorialInstuctionObject m_TutoInstructionObject;
        protected override void OnFoodCollected()
        {
            m_TutoInstructionObject.OnCompleteTask();
            base.OnFoodCollected();
        }
    }
}