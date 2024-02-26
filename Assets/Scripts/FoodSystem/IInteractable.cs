using UnityEngine;

namespace FoodSystem
{
    public interface IInteractable
    {
        public GameObject StartInteraction();
        public string GetContext();
    }
}
