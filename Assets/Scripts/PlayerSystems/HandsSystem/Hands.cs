using System;
using FoodSystem.FoodType;
using UnityEngine;

namespace Player.HandSystem
{
    [Serializable]
    public class Hands
    {
        [SerializeField] HandsType handsType;
        [SerializeField] private Transform foodPosition;
        [SerializeField] private Transform throwPoint;
        private float throwForce;
        private GameObject handledFood;
        private Food currentFood;
        Rigidbody momentumRb;
        [SerializeField] public bool isFoodHandle { get; private set; } = false;

        public void InitData(float throwForce, Rigidbody momentumRb)
        {
            this.throwForce = throwForce;
            this.momentumRb = momentumRb;
        }

        public void PutItHand(GameObject food)
        {
            if(food == null) return;
            
            if(currentFood && currentFood.GetType() == typeof(MergedFood))
            {
                SimpleFood newSimpleFood = food.GetComponent<SimpleFood>();
                
                if(newSimpleFood == null)
                {
                    MergedFood newMergedFood = food.GetComponent<MergedFood>();
                    currentFood.AddFood(newMergedFood);
                }
                else
                {
                    currentFood.AddFood(newSimpleFood);
                }

                UnityEngine.Object.Destroy(food);
            }
            else if(!currentFood)
            {
                //Add a function in food to put and remove in hand
                food.GetComponent<Rigidbody>().velocity = Vector3.zero;
                food.GetComponent<Rigidbody>().isKinematic = true;
                food.GetComponent<BoxCollider>().enabled = false;
                food.transform.SetParent(foodPosition);
                food.transform.position = foodPosition.position;
                food.transform.localRotation = Quaternion.identity;
                SetFood(food);

            }
        }

        public (GameObject, Food) GetHandInfos()
        {
            return (handledFood, currentFood);
        }

        public void ReleaseFood()
        {
            handledFood.GetComponent<Rigidbody>().isKinematic = false;
            handledFood.transform.SetParent(null);
            handledFood.GetComponent<BoxCollider>().enabled = true;
            handledFood.GetComponent<Rigidbody>().AddForce(momentumRb.velocity + throwPoint.forward * throwForce, ForceMode.Impulse);
            SetFood(null);
        }

        public void DestroyFood()
        {
            UnityEngine.Object.Destroy(handledFood);
            SetFood(null);
        }

        void SetFood(GameObject foodGo)
        {
            handledFood = foodGo;
            
            if(foodGo)
            {
                if(foodGo.GetComponent<Food>().GetType() == typeof(SimpleFood))
                {
                    currentFood = (SimpleFood)foodGo.GetComponent<Food>();
                }
                else
                {
                    currentFood = (MergedFood)foodGo.GetComponent<Food>();
                }
            }
            else
            {
                currentFood = null;
            }

            isFoodHandle = foodGo == null ? false : true;
        }
    }

    public enum HandsType
    {
        NONE = 0,
        LEFT = 1, RIGHT = 2
    }
}