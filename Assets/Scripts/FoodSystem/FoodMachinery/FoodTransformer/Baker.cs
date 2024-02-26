using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Baker : FoodTransformer
    {
        protected override void ReleaseFood()
        {
            GameObject newFood = Instantiate(collectedFoodData[0].bakedFood.prefab,
                foodSpawnPoint.position, foodSpawnPoint.rotation);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
        
            base.ReleaseFood();
        }
    }
}
