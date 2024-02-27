using FoodSystem.FoodType;
using UnityEngine;

namespace FoodSystem.FoodMachinery
{
    public abstract class FoodCollector : MonoBehaviour
    {
        protected Food collectedFood;
        protected FoodData[] collectedFoodData;
        protected GameObject collectedFoodGo;

        protected bool canCollect = true;
        [SerializeField] bool collectMergedFood = false;
    
        void OnCollisionEnter(Collision other)
        {
            if (!canCollect) return;
        
            MergedFood tempMergedFood = other.transform.GetComponent<MergedFood>();
            SimpleFood tempSimpleFood = other.transform.GetComponent<SimpleFood>();
        
            if (tempMergedFood != null && collectMergedFood)
            {
                collectedFood = tempMergedFood;
                FoodData[] tempDataArray = tempMergedFood.data.ToArray();
                collectedFoodData = tempDataArray;
            }
            else if (tempSimpleFood != null)
            {
                collectedFood = tempSimpleFood;
                collectedFoodData = new [] { tempSimpleFood.data };
            }
            else
                return;

            collectedFoodGo = other.gameObject;
            OnFoodCollected();
        }

        protected void ResetCollector()
        {
            collectedFood = null;
            collectedFoodData = null;
            collectedFoodGo = null;
        }

        protected abstract void OnFoodCollected();
    }
}
