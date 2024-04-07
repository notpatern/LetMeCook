using Player.Interaction;
using UnityEngine;

namespace FoodSystem.FoodMachinery
{
    public class StoragePlatform : FoodCollector, IInteractable
    {
        [SerializeField] Transform foodSpawn;
        [SerializeField] Collider collisionToEnableOnFoodStocked;
        [SerializeField] GameObject activeParticle;

        void Awake()
        {
            collisionToEnableOnFoodStocked.enabled = false;
            activeParticle.SetActive(false);
        }

        public GameObject StartInteraction()
        {
            if(!collectedFoodGo)
            {
                return null;
            }

            activeParticle.SetActive(false);

            collisionToEnableOnFoodStocked.enabled = false;
            GameObject foodToGive = collectedFoodGo;
            foodToGive.GetComponent<Collider>().enabled = true;
            ResetCollector();
            canCollect = true;
            return foodToGive;
        }

        public string GetContext() 
        {
            return "platform's food";
        }

        protected override void OnFoodCollected()
        {
            activeParticle.SetActive(true);

            collisionToEnableOnFoodStocked.enabled = true;
            Rigidbody rb = collectedFoodGo.GetComponent<Rigidbody>();
            collectedFoodGo.GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;

            collectedFoodGo.transform.SetParent(foodSpawn, false);
            collectedFoodGo.transform.position = Vector3.zero;
            collectedFoodGo.transform.rotation = Quaternion.identity;

            canCollect = false;
        }
    }
}
