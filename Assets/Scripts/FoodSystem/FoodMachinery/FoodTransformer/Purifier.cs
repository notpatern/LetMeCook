using ItemLaunch;
using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    public class Purifier : FoodTransformer
    {
        protected override void ReleaseFood()
        {
            GameObject newFood = Instantiate(collectedFoodData[0].purifiedFood.prefab,
                launcher.StartPoint, Quaternion.identity);

            launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
        
            base.ReleaseFood();
        }

        protected override bool CheckIfCanCook(FoodData foodData) => foodData.purifiedFood != null;
        
    }
}
