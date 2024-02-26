using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Fryer : FoodTransformer
    {
        protected override void ReleaseFood()
        {
            GameObject newFood = Instantiate(collectedFoodData[0].friedFood.prefab,
                foodSpawnPoint.position, foodSpawnPoint.rotation);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
        
            base.ReleaseFood();
        }
    }
}
