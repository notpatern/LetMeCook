using FoodSystem;
using FoodSystem.FoodType;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystems.HandsSystem
{
    [Serializable]
    public class HandsEnableMoveTech
    {
        //All food moveTech
        BoolTable m_IsDashActivated = new BoolTable();
        BoolTable m_IsWallRunActivated = new BoolTable();
        BoolTable m_IsDoubleJumpActivated = new BoolTable();

        bool m_DashDirtyState = false;
        bool m_WallRunDirtyState = false;
        bool m_DoubleJumpDirtyState = false;

        //SimpleFood move tech
        BoolTable m_IsSimpleFoodDashActivated = new BoolTable();
        BoolTable m_IsSimpleFoodWallRunActivated = new BoolTable();
        BoolTable m_IsSimpleFoodDoubleJumpActivated = new BoolTable();

        bool m_IsSimpleFoodDashDirtyState = false;
        bool m_IsSimpleFoodWallRunDirtyState = false;
        bool m_IsSimpleFoodDoubleJumpDirtyState = false;

        UnityEvent<bool> m_OnDashChangeState = new UnityEvent<bool>();
        UnityEvent<bool> m_OnDoubleJumpChangeState = new UnityEvent<bool>();
        UnityEvent<bool> m_OnWallRunChangeState = new UnityEvent<bool>();

        [SerializeField] GameEventScriptableObject m_WallrunGameEventScriptableObject;

        [Header("Game Event SimpleFood MoveTech")]
        [SerializeField] GameEventScriptableObject m_SimpleNotCookedFoodWallrunGameEventScriptableObject;
        [SerializeField] GameEventScriptableObject m_SimpleNotCookedFoodDashGameEventScriptableObject;
        [SerializeField] GameEventScriptableObject m_SimpleNotCookedFoodDoubleJumpGameEventScriptableObject;

        public void LoadMoveTech(FoodData[] foodDatas, bool isSimpleFood)
        {
            UpdateMoveTechState(foodDatas, true, isSimpleFood);
            ApplyOnMoveTechStateChanged();
        }

        public void ClearMoveTech(FoodData[] foodDatas, bool isSimpleFood)
        {
            UpdateMoveTechState(foodDatas, false, isSimpleFood);
            ApplyOnMoveTechStateChanged();
        }

        void UpdateMoveTechState(FoodData[] foodDatas, bool state, bool isSimpleFood)
        {
            foreach (FoodData food in foodDatas)
            {
                if (food.dash)
                {
                    m_IsDashActivated.Value = state;
                }
                if (food.wallRide)
                {
                    m_IsWallRunActivated.Value = state;
                }
                if (food.doubleJump)
                {
                    m_IsDoubleJumpActivated.Value = state;
                }

                if (isSimpleFood)
                {
                    UpdateNotCookedSimpleFoodMoveTechEvent(food, state);
                }
            }

        }

        public void UpdateNotCookedSimpleFoodMoveTechEvent(FoodData simpleFoodData, bool state)
        {
            if(!simpleFoodData.HasNextTransformatedState())
            {
                return;
            }

            if (simpleFoodData.dash)
            {
                m_IsSimpleFoodDashActivated.Value = state;
            }
            if (simpleFoodData.wallRide)
            {
                m_IsSimpleFoodWallRunActivated.Value = state;
            }
            if (simpleFoodData.doubleJump)
            {
                m_IsSimpleFoodDoubleJumpActivated.Value = state;
            }
        }

        public void CallUpdateNotCookedSimpleFoodMoveTechEvent()
        {
            if (m_IsSimpleFoodDashActivated.Value != m_IsSimpleFoodDashDirtyState)
            {
                m_SimpleNotCookedFoodDashGameEventScriptableObject.TriggerEvent(m_IsSimpleFoodDashActivated.Value);
                m_IsSimpleFoodDashDirtyState = m_IsSimpleFoodDashActivated.Value;
            }
            if (m_IsSimpleFoodWallRunActivated.Value != m_IsSimpleFoodWallRunDirtyState)
            {
                m_SimpleNotCookedFoodWallrunGameEventScriptableObject.TriggerEvent(m_IsSimpleFoodWallRunActivated.Value);
                m_IsSimpleFoodWallRunDirtyState = m_IsSimpleFoodWallRunActivated.Value;
            }
            if (m_IsSimpleFoodDoubleJumpActivated.Value != m_IsSimpleFoodDoubleJumpDirtyState)
            {
                m_SimpleNotCookedFoodDoubleJumpGameEventScriptableObject.TriggerEvent(m_IsSimpleFoodDoubleJumpActivated.Value);
                m_IsSimpleFoodDoubleJumpDirtyState = m_IsSimpleFoodDoubleJumpActivated.Value;
            }
        }

        void ApplyOnMoveTechStateChanged()
        {
            if(m_IsDashActivated.Value != m_DashDirtyState)
            {
                m_OnDashChangeState.Invoke(m_IsDashActivated.Value);
                m_DashDirtyState = m_IsDashActivated.Value;
            }
            if(m_IsWallRunActivated.Value != m_WallRunDirtyState)
            {
                m_OnWallRunChangeState.Invoke(m_IsWallRunActivated.Value);

                m_WallrunGameEventScriptableObject.TriggerEvent(m_IsWallRunActivated.Value);//there are 2 types of event right there look for combine them
                m_WallRunDirtyState = m_IsWallRunActivated.Value;
            }
            if(m_IsDoubleJumpActivated.Value != m_DoubleJumpDirtyState)
            {
                m_OnDoubleJumpChangeState.Invoke(m_IsDoubleJumpActivated.Value);
                m_DoubleJumpDirtyState = m_IsDoubleJumpActivated.Value;
            }

            CallUpdateNotCookedSimpleFoodMoveTechEvent();
        }

        public void BindUpdateDashState(UnityAction<bool> action)
        {
            m_OnDashChangeState.AddListener(action);
        }

        public void BindUpdateWallRunState(UnityAction<bool> action)
        {
            m_OnWallRunChangeState.AddListener(action);
        }

        public void BindUpdateDoubleJumpState(UnityAction<bool> action)
        {
            m_OnDoubleJumpChangeState.AddListener(action);
        }
    }
}