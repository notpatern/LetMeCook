using UnityEngine;

namespace FoodSystem.FoodMachinery
{
    public class StoragePlatform : FoodCollector
    {
        [SerializeField] Transform foodSpawn;

        public string GetContext() => collectedFood.GetContext();

        protected override void OnFoodCollected()
        {
            Rigidbody rb = collectedFoodGo.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
        
            collectedFoodGo.transform.position = foodSpawn.position;
            collectedFoodGo.transform.rotation = foodSpawn.rotation;
        }
    }
}
