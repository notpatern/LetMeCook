using FoodSystem.FoodMachinery;
using UnityEngine;

namespace Tutorial {

    public class TutorialDropper : FoodDropper
    {
        [Header("Tuto")]
        [SerializeField] TutorialInstuctionObject m_tutoTask;

        public override GameObject StartInteraction()
        {
            m_tutoTask.OnCompleteTask();
            return base.StartInteraction();
        }
    }
}