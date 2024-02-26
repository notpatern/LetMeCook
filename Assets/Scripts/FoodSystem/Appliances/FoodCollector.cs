using System;
using UnityEngine;

public abstract class FoodCollector : MonoBehaviour
{
    protected Food collectedFood;
    protected GameObject collectedFoodGo;

    [SerializeField] bool canCollectMergedFood = false;
    
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Food"))
        {
            if (other.GetType() == typeof(MergedFood) && !canCollectMergedFood) 
                return;

            collectedFoodGo = other.gameObject;
            collectedFood = other.transform.GetComponent<Food>();
            OnFoodCollected(other.gameObject);
        }
    }

    protected abstract void OnFoodCollected(GameObject collectedFoodGo);
}
