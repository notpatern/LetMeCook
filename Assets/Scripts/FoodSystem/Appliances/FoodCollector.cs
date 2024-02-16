using System;
using UnityEngine;

public abstract class FoodCollector : MonoBehaviour
{
    protected Food collectedFood;

    [SerializeField] bool canCollectMergedFood = false;
    
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Food"))
        {
            if (other.GetType() == typeof(MergedFood))
            {
                if (!canCollectMergedFood)
                    return;

                other.transform.GetComponent<MergedFood>();
            }
        }
    }
}
