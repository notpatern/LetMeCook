using System;
using Player.HandSystem;
using UnityEngine;
using FoodSystem.FoodType;

namespace PlayerSystems.HandsSystem
{
    [Serializable]
    public class HandsManager
    {
        [Header("Throw Food")]
        [SerializeField] private float throwForce;
        [SerializeField] Vector2 throwMomentumForwardDirection = new Vector2(1f, 2f);
        [SerializeField] float throwMomentumPlayerRb = .2f;

        [Header("Hands")]
        [SerializeField] private Hands leftHand;
        [SerializeField] private Hands rightHand;

        [Header("Merged Food")]
        [SerializeField] private GameObject mergedFoodPrefab;

        public void Init(Rigidbody momentumRb)
        {
            leftHand.InitData(throwForce, momentumRb, throwMomentumForwardDirection, throwMomentumPlayerRb);
            rightHand.InitData(throwForce, momentumRb, throwMomentumForwardDirection, throwMomentumPlayerRb);
        }
        
        public void UseHand(GameObject food, HandsType handsType)
        {
            switch (handsType)
            {
                case HandsType.NONE:
                    Debug.LogError("This should not happen but does.");
                    break;
                case HandsType.LEFT:
                    PerformHandAction(food, leftHand);
                    break;
                case HandsType.RIGHT:
                    PerformHandAction(food, rightHand);
                    break;
            }
        }

        public void PerformHandAction(GameObject food, Hands hand)
        {
            if (!hand.isFoodHandle)
            {
                if(food)
                    hand.PutItHand(food);
            }
            else
            {
                hand.ReleaseFood();
            }
        }

        public void MergeFood()
        {
            MergeHandFood(leftHand, rightHand);
        }

        void MergeHandFood(Hands finalMergeHand, Hands movedFoods)
        {
            if(!movedFoods.isFoodHandle) return;

            (GameObject, Food) currentLeftFood = finalMergeHand.GetHandInfos();
            (GameObject, Food) currentRightFood = movedFoods.GetHandInfos();

            if(!finalMergeHand.isFoodHandle)
            {
                finalMergeHand.PutItHand(UnityEngine.Object.Instantiate(mergedFoodPrefab));
                finalMergeHand.PutItHand(currentRightFood.Item1);
                movedFoods.DestroyFood();
            }
            else if(currentLeftFood.Item2.GetType() == typeof(SimpleFood))
            {
                ReplaceSimpleFoodHandWithMergedFood(finalMergeHand, (SimpleFood)currentLeftFood.Item2, movedFoods, currentRightFood.Item1);   
            }
            else if(currentLeftFood.Item2.GetType() == typeof(MergedFood))
            {
                AddFoodInHand(currentLeftFood.Item2, currentRightFood.Item2);
                movedFoods.DestroyFood();
            }
            else
            {
                Debug.LogError("MergeFood possibility not handled");
            }
        }

        void AddFoodInHand(Food finalFood, Food foodToAdd)
        {
            MergedFood food = (MergedFood)finalFood;
            SimpleFood foodSimpleChild = (SimpleFood)foodToAdd;

            if(foodSimpleChild == null)
            {
                MergedFood foodMergeChild = (MergedFood)foodToAdd;
                finalFood.AddFood(foodMergeChild);
            }
            else
            {
                food.AddFood(foodSimpleChild);
            }
        }

        void ReplaceSimpleFoodHandWithMergedFood(Hands handToReplace, SimpleFood simpleToReplace, Hands otherHand, GameObject newGoFood)
        {
            //Replace simpleFood in hand with a Merged food
            GameObject handMergedGo = UnityEngine.Object.Instantiate(mergedFoodPrefab);
            MergedFood mergedFood = handMergedGo.GetComponent<MergedFood>();
            mergedFood.AddFood(simpleToReplace);

            handToReplace.DestroyFood();
            handToReplace.PutItHand(handMergedGo);

            //Add right hand in merged left hand
            handToReplace.PutItHand(newGoFood);
            otherHand.DestroyFood();
        }
    }

}