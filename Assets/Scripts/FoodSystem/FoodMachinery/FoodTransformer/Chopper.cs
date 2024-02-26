using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Chopper : FoodTransformer
    {
        protected override void ReleaseFood()
        {
            GameObject newFood = Instantiate(collectedFoodData[0].choppedFood.prefab,
                foodSpawnPoint.position, foodSpawnPoint.rotation);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
        
            base.ReleaseFood();
        }
    }
}
