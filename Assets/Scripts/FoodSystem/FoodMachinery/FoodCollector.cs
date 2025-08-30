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
        [SerializeField] protected bool collectMergedFood = false;

        private void OnTriggerStay(Collider other)
        {
            TriggerEnter(other);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            TriggerEnter(other);
        }

        void TriggerEnter(Collider other)
        {
            if (!canCollect) return;

            MergedFood tempMergedFood = other.transform.GetComponent<MergedFood>();
            SimpleFood tempSimpleFood = other.transform.GetComponent<SimpleFood>();

            if (tempMergedFood != null && collectMergedFood)
            {
                collectedFood = tempMergedFood;
                FoodData[] tempDataArray = tempMergedFood.GetFoodDatas().ToArray();
                collectedFoodData = tempDataArray;
            }
            else if (tempSimpleFood != null)
            {
                collectedFood = tempSimpleFood;
                collectedFoodData = new[] { tempSimpleFood.data };
            }
            else
                return;

            collectedFoodGo = other.gameObject;
            collectedFoodGo.transform.rotation = Quaternion.identity;
            OnFoodCollected();
        }

        protected virtual void UpdateFoodInRange(Food food, int amount) { }

        protected void ResetCollector()
        {
            collectedFood = null;
            collectedFoodData = null;
            collectedFoodGo = null;
        }

        protected abstract void OnFoodCollected();
    }
}
