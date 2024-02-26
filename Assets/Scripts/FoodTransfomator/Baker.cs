using UnityEngine;

public class Baker : FoodTransformator
{
    protected override void ReleaseFood()
    {
        GameObject newFood = Instantiate(collectedFoodData[0].bakedFood.prefab,
            foodSpawnPoint.position, foodSpawnPoint.rotation);
        
        if (newFood == null)
            Debug.LogWarning("OK");
        
        //newFood.GetComponent<Rigidbody>().AddForce(foodSpawnPoint.forward * releaseForce, ForceMode.Impulse);
        launcher.ThrowItem(newFood.GetComponent<LaunchableItem>());
        
        base.ReleaseFood();
    }
}
