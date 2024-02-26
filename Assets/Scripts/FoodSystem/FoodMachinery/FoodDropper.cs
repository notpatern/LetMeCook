using FoodSystem.FoodType;
using UnityEngine;

namespace FoodSystem.FoodMachinery
{
    public class FoodDropper : MonoBehaviour, IInteractable
    {
        [SerializeField] SimpleFood givingFood;
    
        public GameObject StartInteraction()
        {
            //TODO give food to the player's hand
            return givingFood.data.prefab;
        }

        public string GetContext() => "dropper (" + givingFood.GetContext() + ")";
    }
}
