using Player.Interaction;
using UnityEngine;

namespace FoodSystem.FoodMachinery
{
    public class StoragePlatform : FoodCollector, IInteractable
    {
        [SerializeField] Transform foodSpawn;
        IInteractable _interactableImplementation;

        public GameObject StartInteraction()
        {
            //TODO Give food to the player's hand
            canCollect = true;
            return collectedFoodGo;
        }

        public string GetContext() => collectedFood.GetContext();

        protected override void OnFoodCollected()
        {
            Rigidbody rb = collectedFoodGo.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
        
            collectedFoodGo.transform.position = foodSpawn.position;
            collectedFoodGo.transform.rotation = foodSpawn.rotation;

            canCollect = false;
        }
    }
}
