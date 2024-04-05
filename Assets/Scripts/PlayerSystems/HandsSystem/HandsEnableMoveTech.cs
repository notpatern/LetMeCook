using FoodSystem;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystems.HandsSystem
{
    [Serializable]
    public class HandsEnableMoveTech
    {
        BoolTable m_IsDashActivated = new BoolTable();
        BoolTable m_IsWallRunActivated = new BoolTable();
        BoolTable m_IsDoubleJumpActivated = new BoolTable();

        bool m_DashDirtyState = false;
        bool m_WallRunDirtyState = false;
        bool m_DoubleJumpDirtyState = false;

        UnityEvent<bool> m_OnDashChangeState = new UnityEvent<bool>();
        UnityEvent<bool> m_OnDoubleJumpChangeState = new UnityEvent<bool>();
        UnityEvent<bool> m_OnWallRunChangeState = new UnityEvent<bool>();

        [SerializeField] GameEventScriptableObject m_WallunGameEventScriptableObject;

        public void LoadMoveTech(FoodData[] foodDatas)
        {
            UpdateMoveTechState(foodDatas, true);
            ApplyOnMoveTechStateChanged();
        }

        public void ClearMoveTech(FoodData[] foodDatas)
        {
            UpdateMoveTechState(foodDatas, false);
            ApplyOnMoveTechStateChanged();
        }

        void UpdateMoveTechState(FoodData[] foodDatas, bool state)
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
                m_WallunGameEventScriptableObject.TriggerEvent(m_IsWallRunActivated.Value);//there are 2 types of event right there look for combine them
                m_WallRunDirtyState = m_IsWallRunActivated.Value;
            }
            if(m_IsDoubleJumpActivated.Value != m_DoubleJumpDirtyState)
            {
                m_OnDoubleJumpChangeState.Invoke(m_IsDoubleJumpActivated.Value);
                m_DoubleJumpDirtyState = m_IsDoubleJumpActivated.Value;
            }
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