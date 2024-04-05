using Player.Interaction;
using UnityEngine;

namespace FoodSystem.FoodMachinery
{
    public class StoragePlatform : FoodCollector, IInteractable
    {
        [SerializeField] Transform foodSpawn;
        [SerializeField] Collider collisionToEnableOnFoodStocked;

        void Awake()
        {
            collisionToEnableOnFoodStocked.enabled = false;
        }

        public GameObject StartInteraction()
        {
            if(!collectedFoodGo)
            {
                return null;
            }

            collisionToEnableOnFoodStocked.enabled = false;
            GameObject foodToGive = collectedFoodGo;
            foodToGive.GetComponent<Collider>().enabled = true;
            ResetCollector();
            canCollect = true;
            return foodToGive;
        }

        public string GetContext() 
        {
            return collectedFood ? collectedFood.GetContext() : "Nothing on the platform";
        }

        protected override void OnFoodCollected()
        {
            collisionToEnableOnFoodStocked.enabled = true;
            Rigidbody rb = collectedFoodGo.GetComponent<Rigidbody>();
            collectedFoodGo.GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;

            collectedFoodGo.transform.position = foodSpawn.position;
            collectedFoodGo.transform.rotation = foodSpawn.rotation;

            canCollect = false;
        }
    }
}
