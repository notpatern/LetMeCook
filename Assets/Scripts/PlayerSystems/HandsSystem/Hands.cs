using System;
using FoodSystem.FoodType;
using UnityEngine;

namespace Player.HandSystem
{
    [Serializable]
    public class Hands
    {
        [SerializeField] private Transform m_FoodPosition;
        [SerializeField] private Transform m_ThrowPoint;
        private float m_ThrowForce;
        private GameObject m_HhandledFood;
        private Food m_CurrentFood;

        Rigidbody m_MomentumRb;
        Vector2 m_ThrowMomentumForwardDirection;
        float m_ThrowMomentumPlayerRb;
        [SerializeField] public bool isFoodHandle { get; private set; } = false;

        public void InitData(float throwForce, Rigidbody momentumRb, Vector2 throwMomentumForwardDirection, float throwMomentumPlayerRb)
        {
            m_ThrowForce = throwForce;
            m_MomentumRb = momentumRb;
            m_ThrowMomentumForwardDirection = throwMomentumForwardDirection;
            m_ThrowMomentumPlayerRb = throwMomentumPlayerRb;
        }

        public void PutItHand(GameObject food)
        {
            if(food == null) return;
            
            if(m_CurrentFood && m_CurrentFood.GetType() == typeof(MergedFood))
            {
                SimpleFood newSimpleFood = food.GetComponent<SimpleFood>();
                
                if(newSimpleFood == null)
                {
                    MergedFood newMergedFood = food.GetComponent<MergedFood>();
                    m_CurrentFood.AddFood(newMergedFood);
                }
                else
                {
                    m_CurrentFood.AddFood(newSimpleFood);
                }

                UnityEngine.Object.Destroy(food);
            }
            else if(!m_CurrentFood)
            {
                food.GetComponent<Food>().PutInHand(m_FoodPosition);
                SetFood(food);

            }
        }

        public (GameObject, Food) GetHandInfos()
        {
            return (m_HhandledFood, m_CurrentFood);
        }

        public void ReleaseFood()
        {
            Food food = m_HhandledFood.GetComponent<Food>();
            food.RemoveFromHand();

            Vector3 momentum = m_ThrowPoint.forward * m_ThrowMomentumForwardDirection.x + m_ThrowPoint.up * m_ThrowMomentumForwardDirection.y;
            food.LaunchFood(momentum * m_ThrowForce + m_MomentumRb.velocity * m_ThrowMomentumPlayerRb);
            SetFood(null);
        }

        public void DestroyFood()
        {
            UnityEngine.Object.Destroy(m_HhandledFood);
            SetFood(null);
        }

        void SetFood(GameObject foodGo)
        {
            m_HhandledFood = foodGo;
            
            if(foodGo)
            {
                if(foodGo.GetComponent<Food>().GetType() == typeof(SimpleFood))
                {
                    m_CurrentFood = (SimpleFood)foodGo.GetComponent<Food>();
                }
                else
                {
                    m_CurrentFood = (MergedFood)foodGo.GetComponent<Food>();
                }
            }
            else
            {
                m_CurrentFood = null;
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