using FoodSystem.FoodType;
using ItemLaunch;
using ParticleSystemUtility;
using Player.Interaction;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FoodSystem.FoodMachinery
{
    public class StoragePlatform : FoodCollector, IInteractable
    {
        [SerializeField] Transform foodSpawn;
        [SerializeField] Collider collisionToEnableOnFoodStocked;
        [SerializeField] Collider interactionTrigger;
        [SerializeField] ParticleInstanceManager activeParticle;
        [SerializeField] GameObject energyStockagePaltform;
        
        [Header("Beacon properties")]
        [SerializeField] GameObject zoneIndicatorBeacon;
        Material zoneIndicatorBeaconMaterialInstance;
        [SerializeField] Renderer lightInfoRenderer; 

        void Awake()
        {
            collectMergedFood = true;
            collisionToEnableOnFoodStocked.enabled = false;
            interactionTrigger.enabled = true;
            energyStockagePaltform.SetActive(false);
            activeParticle.Stop(false);
            zoneIndicatorBeaconMaterialInstance = zoneIndicatorBeacon.GetComponent<Renderer>().material;
        }

        public virtual GameObject StartInteraction()
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
            energyStockagePaltform.SetActive(false);
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
            zoneIndicatorBeaconMaterialInstance.SetColor("_IntersectionColor", collectedFood.foodColor);
            energyStockagePaltform.SetActive(true);
            lightInfoRenderer.material.SetColor("_IntersectionColor", collectedFood.foodColor);
            collectedFoodGo.GetComponent<LaunchableItem>().QuitBezierCurve(false);
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
