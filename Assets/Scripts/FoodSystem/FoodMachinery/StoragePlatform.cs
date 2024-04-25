using ParticleSystemUtility;
using Player.Interaction;
using UnityEngine;

namespace FoodSystem.FoodMachinery
{
    public class StoragePlatform : FoodCollector, IInteractable
    {
        [SerializeField] Transform foodSpawn;
        [SerializeField] Collider collisionToEnableOnFoodStocked;
        [SerializeField] Collider interactionTrigger;
        [SerializeField] ParticleInstanceManager activeParticle;

        void Awake()
        {
            collectMergedFood = true;
            collisionToEnableOnFoodStocked.enabled = false;
            interactionTrigger.enabled = true;
            activeParticle.Stop(false);
        }

        public GameObject StartInteraction()
        {
            if(!collectedFoodGo)
            {
                return null;
            }

            activeParticle.Stop(false);

            collisionToEnableOnFoodStocked.enabled = false;
            interactionTrigger.enabled = true;
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
            activeParticle.Play();

            collisionToEnableOnFoodStocked.enabled = true;
            Rigidbody rb = collectedFoodGo.GetComponent<Rigidbody>();
            collectedFoodGo.GetComponent<Collider>().enabled = false;
            interactionTrigger.enabled = false;
            rb.isKinematic = true;

            collectedFoodGo.transform.position = Vector3.zero;
            collectedFoodGo.transform.rotation = Quaternion.identity;
            collectedFoodGo.transform.SetParent(foodSpawn, false);
            Debug.Log("colected");
            canCollect = false;
        }
    }
}
