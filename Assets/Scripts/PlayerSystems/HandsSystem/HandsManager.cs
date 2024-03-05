using System;
using Player.HandSystem;
using UnityEngine;
using FoodSystem.FoodType;
using UnityEngine.Events;

namespace PlayerSystems.HandsSystem
{
    [Serializable]
    public class HandsManager
    {
        [Header("Throw Food")]
        [SerializeField] private float m_ThrowForce;
        [SerializeField] Vector2 m_ThrowMomentumForwardDirection = new Vector2(1f, 2f);
        [SerializeField] float m_ThrowMomentumPlayerRb = .2f;

        [Header("Hands")]
        [SerializeField] private Hands m_LeftHand;
        [SerializeField] private Hands m_RightHand;

        [Header("Merged Food")]
        [SerializeField] private GameObject m_MergedFoodPrefab;
        HandsEnableMoveTech m_HandsEnableMoveTech;

        public void Init(Rigidbody momentumRb)
        {
            m_HandsEnableMoveTech = new HandsEnableMoveTech();
            m_LeftHand.InitData(m_ThrowForce, momentumRb, m_ThrowMomentumForwardDirection, m_ThrowMomentumPlayerRb);
            m_RightHand.InitData(m_ThrowForce, momentumRb, m_ThrowMomentumForwardDirection, m_ThrowMomentumPlayerRb);
        }
        
        public void UseHand(GameObject food, HandsType handsType)
        {
            switch (handsType)
            {
                case HandsType.NONE:
                    Debug.LogError("This should not happen but does.");
                    break;
                case HandsType.LEFT:
                    PerformHandAction(food, m_LeftHand);
                    break;
                case HandsType.RIGHT:
                    PerformHandAction(food, m_RightHand);
                    break;
            }
        }

        public void PerformHandAction(GameObject food, Hands hand)
        {
            if (!hand.isFoodHandle)
            {
                if(food)
                    PutInHand(food, hand, true);
            }
            else
            {
                ReleaseFromHand(hand);
            }
        }

        public void MergeFood()
        {
            MergeHandFood(m_LeftHand, m_RightHand);
        }

        //Bind Actions-----
        public void BindUpdateDashState(UnityAction<bool> action)
        {
            m_HandsEnableMoveTech.BindUpdateDashState(action);
        }

        public void BindUpdateWallRunState(UnityAction<bool> action)
        {
            m_HandsEnableMoveTech.BindUpdateWallRunState(action);
        }

        public void BindUpdateDoubleJumpState(UnityAction<bool> action)
        {
            m_HandsEnableMoveTech.BindUpdateDoubleJumpState(action);
        }
        //-----------------

        void PutInHand(GameObject food, Hands hand, bool activeMoveTechChecker) 
        {
            hand.PutItHand(food);

            if(activeMoveTechChecker)
            {
                m_HandsEnableMoveTech.LoadMoveTech(hand.GetHandFood().GetFoodDatas().ToArray());
            }
        }

        void ReleaseFromHand(Hands hand)
        {
            m_HandsEnableMoveTech.ClearMoveTech(hand.GetHandFood().GetFoodDatas().ToArray());
            hand.ReleaseFood();
        }

        void MergeHandFood(Hands finalMergeHand, Hands movedHand)
        {
            if(!movedHand.isFoodHandle) return;

            (GameObject, Food) currentFinalPosHandData = finalMergeHand.GetHandInfos();
            (GameObject, Food) currentMovedPosHandData = movedHand.GetHandInfos();

            if(!finalMergeHand.isFoodHandle)
            {
                //PutInHand(UnityEngine.Object.Instantiate(m_MergedFoodPrefab), finalMergeHand);
                PutInHand(currentMovedPosHandData.Item1, finalMergeHand, false);
                movedHand.SetFood(null);
            }
            else if(currentFinalPosHandData.Item2.GetType() == typeof(SimpleFood))
            {
                ReplaceSimpleFoodHandWithMergedFood(finalMergeHand, (SimpleFood)currentFinalPosHandData.Item2, movedHand, currentMovedPosHandData.Item1);   
            }
            else if(currentFinalPosHandData.Item2.GetType() == typeof(MergedFood))
            {
                AddFoodInHand(currentFinalPosHandData.Item2, currentMovedPosHandData.Item2);
                movedHand.DestroyFood();
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
            GameObject handMergedGo = UnityEngine.Object.Instantiate(m_MergedFoodPrefab);
            MergedFood mergedFood = handMergedGo.GetComponent<MergedFood>();
            mergedFood.AddFood(simpleToReplace);

            handToReplace.DestroyFood();
            PutInHand(handMergedGo, handToReplace, false);

            //Add right hand in merged left hand
            PutInHand(newGoFood, handToReplace, false);
            otherHand.DestroyFood();
        }
    }

}