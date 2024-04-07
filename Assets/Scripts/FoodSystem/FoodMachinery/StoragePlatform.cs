using ParticleSystemUtility;
using Player.Interaction;
using UnityEngine;

namespace FoodSystem.FoodMachinery
{
    public class StoragePlatform : FoodCollector, IInteractable
    {
        [SerializeField] Transform foodSpawn;
        [SerializeField] Collider collisionToEnableOnFoodStocked;
        [SerializeField] ParticleInstanceManager activeParticle;

        void Awake()
        {
            collisionToEnableOnFoodStocked.enabled = false;
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
            rb.isKinematic = true;

            collectedFoodGo.transform.SetParent(foodSpawn, false);
            collectedFoodGo.transform.localPosition = Vector3.zero;
            collectedFoodGo.transform.localRotation = Quaternion.identity;

            canCollect = false;
        }
    }
}
