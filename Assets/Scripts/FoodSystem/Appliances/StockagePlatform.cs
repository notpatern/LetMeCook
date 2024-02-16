using UnityEngine;

public class StockagePlatform : FoodCollector
{
    [SerializeField] Transform foodSpawn;

    public string GetContext() => collectedFood.GetContext();

    protected override void OnFoodCollected(GameObject foodObject)
    {
        Rigidbody rb = foodObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        
        foodObject.transform.position = foodSpawn.position;
        foodObject.transform.rotation = foodSpawn.rotation;
    }
}
