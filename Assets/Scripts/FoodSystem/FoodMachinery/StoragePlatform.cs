using FoodSystem.FoodType;
using ItemLaunch;
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
            foodToGive.GetComponent<Food>().SetActiveColliders(true);
            collectedFoodGo.GetComponent<Food>().SetActiveTrails(true);
            ResetCollector();
            canCollect = true;
            return foodToGive;
        }

        public string GetContext() 
        {
            if(collectedFood)
            {
                return collectedFood.GetContext();
            }

            return "Empty platform";
        }

        protected override void OnFoodCollected()
        {
            collectedFoodGo.GetComponent<LaunchableItem>().QuitBezierCurve();
            activeParticle.Play();

            collisionToEnableOnFoodStocked.enabled = true;
            Rigidbody rb = collectedFoodGo.GetComponent<Rigidbody>();
            collectedFoodGo.GetComponent<Food>().SetActiveColliders(false);
            collectedFoodGo.GetComponent<Food>().SetActiveTrails(false);
            interactionTrigger.enabled = false;
            rb.isKinematic = true;

            collectedFoodGo.transform.position = Vector3.zero;
            collectedFoodGo.transform.rotation = Quaternion.identity;
            collectedFoodGo.transform.SetParent(foodSpawn, false);
            canCollect = false;
        }
    }
}
