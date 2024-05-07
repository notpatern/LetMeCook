using System;
using Player.HandSystem;
using UnityEngine;
using FoodSystem.FoodType;
using UnityEngine.Events;
using Audio;
using RecipeSystem;

namespace PlayerSystems.HandsSystem
{
    [Serializable]
    public class HandsManager
    {
        [Header("Throw Food")]
        [SerializeField] float m_ThrowForce;
        [SerializeField] Vector2 m_ThrowMomentumForwardDirection = new Vector2(1f, 2f);
        [SerializeField] float m_ThrowMomentumPlayerRb = .2f;

        [Header("Hands")]
        [SerializeField] GameEventScriptableObject m_GameEventCanSpawnMagicalFogForMerge;
        [SerializeField] GameEventScriptableObject m_OnFoodPickupGameEvent;
        [SerializeField] GameObject m_GrabbedFoodParticlePrefab;
        [SerializeField] Hands m_LeftHand;
        [SerializeField] Hands m_RightHand;

        [Header("Merged Food")]
        [SerializeField] GameObject m_MergedFoodPrefab;
        [SerializeField] HandsEnableMoveTech m_HandsEnableMoveTech;

        [SerializeField] Material m_DefaultGemBraceletVisualMaterial;
        [SerializeField] GemBraceletVisual[] m_GemBraceletMoveTechVisual;

        Transform m_CameraTr;
        RecipesManager m_RecipeManager;

        public void Init(Rigidbody momentumRb, Animator playerPrefabAnimator, Transform cameraTr, RecipesManager recipeManager)
        {
            m_CameraTr = cameraTr;
            m_RecipeManager = recipeManager;
            m_LeftHand.InitData(m_ThrowForce, momentumRb, m_ThrowMomentumForwardDirection, m_ThrowMomentumPlayerRb, playerPrefabAnimator, m_GrabbedFoodParticlePrefab, cameraTr, ClearHandMoveTech);
            m_RightHand.InitData(m_ThrowForce, momentumRb, m_ThrowMomentumForwardDirection, m_ThrowMomentumPlayerRb, playerPrefabAnimator, m_GrabbedFoodParticlePrefab, cameraTr, ClearHandMoveTech);

            BindMoveTechVisualEffect();
        }

        void BindMoveTechVisualEffect()
        {
            if(m_GemBraceletMoveTechVisual.Length != 3)
            {
                Debug.LogError("MoveTech Bracelet visual not handle withour 3 visuals");
            }

            for (int i = 0; i < m_GemBraceletMoveTechVisual.Length; i++)
            {
                m_GemBraceletMoveTechVisual[i].ChangeGemsMat(m_DefaultGemBraceletVisualMaterial);
            }

            BindUpdateDashState(action => 
            {
                UpdateGemBraceletVisual(action, 0);
            });

            BindUpdateWallRunState(action => 
            {
                UpdateGemBraceletVisual(action, 1);
            });

            BindUpdateDoubleJumpState(action => 
            {
                UpdateGemBraceletVisual(action, 2);
            });
        }

        void UpdateGemBraceletVisual(bool isActive, int gemBraceletId)
        {
            if (isActive)
            {
                m_GemBraceletMoveTechVisual[gemBraceletId].ChangeGemsMat();
            }
            else
            {
                m_GemBraceletMoveTechVisual[gemBraceletId].ChangeGemsMat(m_DefaultGemBraceletVisualMaterial);
            }
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

        public bool IsFoodHandle(HandsType handsType)
        {

            switch (handsType)
            {
                case HandsType.NONE:
                    Debug.LogError("This should not happen but does.");
                    return false;
                case HandsType.LEFT:
                    return m_LeftHand.isFoodHandle;
                case HandsType.RIGHT:
                    return m_RightHand.isFoodHandle;
            }

            return false;
        }

        public bool IsFoodInHand(HandsType handsType, GameObject food)
        {
            switch(handsType)
            {
                case HandsType.NONE:
                    Debug.LogError("This should not happen but does.");
                    return false;

                case HandsType.LEFT:
                    if (!IsFoodHandle(HandsType.LEFT)) break;
                    return IsFoodInCurrentHand(m_LeftHand, food);

                case HandsType.RIGHT:
                    if (!IsFoodHandle(HandsType.RIGHT)) break;
                    return IsFoodInCurrentHand(m_RightHand, food);
            }
            return false;
        }

        private bool IsFoodInCurrentHand(Hands hand, GameObject food)
        {
            return hand.GetHandFood().gameObject == food;
        }

        public void PerformHandAction(GameObject food, Hands hand)
        {
            if (hand.m_IsCrushing) return; 

            if (!hand.isFoodHandle)
            {
                if (food)
                {
                    m_OnFoodPickupGameEvent.TriggerEvent(food);
                    PutInHand(food, hand, true, true, false);
                }
            }
            else
            {
                ReleaseFromHand(hand);
                AudioManager.s_Instance.PlayOneShot(AudioManager.s_Instance.m_AudioSoundData.m_PlayerThrowSound, m_CameraTr.position);
            }
        }

        public void MergeFood()
        {
            if (!m_RightHand.m_IsCrushing && !m_LeftHand.m_IsCrushing)
            {
                MergeHandFood(m_LeftHand, m_RightHand);
            }
        }

        public void CrunchFoodInHands(bool forceAnim)
        {
            if (!forceAnim)
            {
                if(m_LeftHand.isFoodHandle || m_RightHand.isFoodHandle)
                {
                    CrunchHands();
                }
            }
            else
            {
                CrunchHands();
            }
        }

        void CrunchHands()
        {
            m_LeftHand.m_IsCrushing = true;
            m_RightHand.m_IsCrushing = true;

            m_LeftHand.m_Animator.SetTrigger("CrunchFood");
            m_RightHand.m_Animator.SetTrigger("CrunchFood");
        }

        public void ClearHandMoveTech(Hands hand)
        {
            if (hand.GetHandFood())
            {
                bool isSimpleFood = hand.GetHandFood().GetType() == typeof(SimpleFood);
                m_HandsEnableMoveTech.ClearMoveTech(hand.GetHandFood().GetFoodDatas().ToArray(), isSimpleFood);
            }
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

        void PutInHand(GameObject food, Hands hand, bool activeMoveTechChecker, bool grabAnim, bool forceParticles) 
        {
            hand.PutItHand(food, grabAnim, forceParticles);

            if(activeMoveTechChecker)
            {
                bool isSimpleFood = hand.GetHandFood().GetType() == typeof(SimpleFood);
                m_HandsEnableMoveTech.LoadMoveTech(hand.GetHandFood().GetFoodDatas().ToArray(), isSimpleFood);
            }
        }

        void ReleaseFromHand(Hands hand)
        {
            bool isSimpleFood = hand.GetHandFood().GetType() == typeof(SimpleFood);

            m_HandsEnableMoveTech.ClearMoveTech(hand.GetHandFood().GetFoodDatas().ToArray(), isSimpleFood);
            hand.ReleaseFood();
        }

        void MergeHandFood(Hands finalMergeHand, Hands movedHand)
        {
            if(!movedHand.isFoodHandle) return;

            (GameObject, Food) currentFinalPosHandData = finalMergeHand.GetHandInfos();
            (GameObject, Food) currentMovedPosHandData = movedHand.GetHandInfos();

            Type finalMergedHandType = currentFinalPosHandData.Item1 ? currentFinalPosHandData.Item2.GetType() : null;
            Type movedHandType = currentMovedPosHandData.Item2.GetType();

            m_GameEventCanSpawnMagicalFogForMerge.TriggerEvent(true);
            if (!finalMergeHand.isFoodHandle)
            {
                //PutInHand(UnityEngine.Object.Instantiate(m_MergedFoodPrefab), finalMergeHand);
                PutInHand(currentMovedPosHandData.Item1, finalMergeHand, false, true, false);
                movedHand.SetFood(null, false, false);

                m_GameEventCanSpawnMagicalFogForMerge.TriggerEvent(false);
            }
            else if(finalMergedHandType == typeof(SimpleFood))
            {
                SimpleFood simpleFood = (SimpleFood)currentFinalPosHandData.Item2;
                m_HandsEnableMoveTech.UpdateNotCookedSimpleFoodMoveTechEvent(simpleFood.data, false);

                
                if (movedHandType == typeof(SimpleFood))
                {
                    SimpleFood secondSimpleFood = (SimpleFood)currentMovedPosHandData.Item2;
                    m_HandsEnableMoveTech.UpdateNotCookedSimpleFoodMoveTechEvent(secondSimpleFood.data, false);
                }

                m_HandsEnableMoveTech.CallUpdateNotCookedSimpleFoodMoveTechEvent();
                ReplaceSimpleFoodHandWithMergedFood(finalMergeHand, (SimpleFood)currentFinalPosHandData.Item2, movedHand, currentMovedPosHandData.Item1);   
            }
            else if(finalMergedHandType == typeof(MergedFood))
            {
                if (movedHandType == typeof(SimpleFood))
                {
                    SimpleFood secondSimpleFood = (SimpleFood)currentMovedPosHandData.Item2;
                    m_HandsEnableMoveTech.UpdateNotCookedSimpleFoodMoveTechEvent(secondSimpleFood.data, false);
                    m_HandsEnableMoveTech.CallUpdateNotCookedSimpleFoodMoveTechEvent();
                }

                finalMergeHand.PutItHand(currentMovedPosHandData.Item1, true, false);
                //AddFoodInHand(currentFinalPosHandData.Item2, currentMovedPosHandData.Item2);
                movedHand.DestroyFood();
            }
            else
            {
                Debug.LogError("MergeFood possibility not handled");
            }

            movedHand.m_Animator.SetTrigger("StartMerge");
            finalMergeHand.m_Animator.SetTrigger("FinalMerge");
        }

        /*void AddFoodInHand(Food finalFood, Food foodToAdd)
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
        }*/

        void ReplaceSimpleFoodHandWithMergedFood(Hands handToReplace, SimpleFood simpleToReplace, Hands otherHand, GameObject newGoFood)
        {
            //Replace simpleFood in hand with a Merged food
            GameObject handMergedGo = UnityEngine.Object.Instantiate(m_MergedFoodPrefab);
            MergedFood mergedFood = handMergedGo.GetComponent<MergedFood>();
            mergedFood.Init(m_RecipeManager);
            mergedFood.AddFood(simpleToReplace);

            handToReplace.DestroyFood();
            PutInHand(handMergedGo, handToReplace, false, false, true);

            //Add right hand in merged left hand
            PutInHand(newGoFood, handToReplace, false, false, false);
            otherHand.DestroyFood();
        }
    }

    [Serializable]
    public class GemBraceletVisual
    {
        [SerializeField] MeshRenderer[] m_Gems;
        [SerializeField] Material m_DefaultActiveGemMat;

        public void ChangeGemsMat(Material newMat = null)
        {
            if(!newMat)
            {
                newMat = m_DefaultActiveGemMat;
            }

            foreach (MeshRenderer gem in m_Gems)
            {
                gem.material = newMat;
            }
        }
    }

}