using UnityEngine;

namespace Player.Interaction
{
    public interface IInteractable
    {
        public GameObject StartInteraction();
        public string GetContext();
    }
}
