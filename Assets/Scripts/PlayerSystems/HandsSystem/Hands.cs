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


        [SerializeField] private Transform m_EffectSpawnPoint; 
        public Animator m_Animator;
        [SerializeField] private HandAnimatorManagement m_HandAnimatorManagement;
        ParticleSystemUtility.ParticleInstanceManager m_ParticleInstanceManager;

        private float m_ThrowForce;
        private GameObject m_HhandledFood;
        private Food m_CurrentFood;

        Rigidbody m_MomentumRb;
        Vector2 m_ThrowMomentumForwardDirection;
        float m_ThrowMomentumPlayerRb;
        public bool isFoodHandle { get; private set; } = false;
        Animator m_PlayerPrefabAnimator;
        float m_IdleHandAnimSpeed;
        int m_IdleHashFullPathPlayerPrefabForSync;
        GameObject m_GrabbedFoodParticlePrefab;

        public void InitData(float throwForce, Rigidbody momentumRb, Vector2 throwMomentumForwardDirection, float throwMomentumPlayerRb, Animator playerPrefabAnimator, GameObject grabbedFoodParticle)
        {
            m_ThrowForce = throwForce;
            m_MomentumRb = momentumRb;
            m_ThrowMomentumForwardDirection = throwMomentumForwardDirection;
            m_ThrowMomentumPlayerRb = throwMomentumPlayerRb;

            m_GrabbedFoodParticlePrefab = grabbedFoodParticle;

            m_PlayerPrefabAnimator = playerPrefabAnimator;
            m_IdleHandAnimSpeed = m_Animator.GetCurrentAnimatorStateInfo(0).speed;
            m_IdleHashFullPathPlayerPrefabForSync = m_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            m_HandAnimatorManagement.BindResincronyzationOnMainIdle(ResincronyzationOnMainIdleAnimation);
            m_HandAnimatorManagement.BindStopGrabParticle(RemoveFoodEffect);
        }

        public void PutItHand(GameObject food, bool grabAnim)
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
                SetFood(food, grabAnim);

            }
        }

        public (GameObject, Food) GetHandInfos()
        {
            return (m_HhandledFood, m_CurrentFood);
        }

        public Food GetHandFood()
        {
            return m_CurrentFood;
        }

        public void ReleaseFood()
        {
            ThrowFoodVisualEffect();

            Food food = m_HhandledFood.GetComponent<Food>();
            food.RemoveFromHand();

            Vector3 momentum = m_ThrowPoint.forward * m_ThrowMomentumForwardDirection.x + m_ThrowPoint.up * m_ThrowMomentumForwardDirection.y;
            food.LaunchFood(momentum * m_ThrowForce + m_MomentumRb.velocity * m_ThrowMomentumPlayerRb);
            SetFood(null, false);
        }

        public void DestroyFood()
        {
            UnityEngine.Object.Destroy(m_HhandledFood);
            SetFood(null, false);
        }

        public void SetFood(GameObject foodGo, bool grabAnim)
        {
            m_HhandledFood = foodGo;

            if (foodGo)
            {
                if (grabAnim)
                {
                    GrabFoodVisualEffect();
                }

                if (foodGo.GetComponent<Food>().GetType() == typeof(SimpleFood))
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

        void GrabFoodVisualEffect()
        {
            m_Animator.SetTrigger("GrabFood");

            if (!m_ParticleInstanceManager)
            {
                m_ParticleInstanceManager = UnityEngine.Object.Instantiate(m_GrabbedFoodParticlePrefab, m_EffectSpawnPoint).GetComponent<ParticleSystemUtility.ParticleInstanceManager>();
            }

            m_ParticleInstanceManager.Emit(1);
        }

        void ThrowFoodVisualEffect()
        {
            m_Animator.SetTrigger("Throw");
        }

        void RemoveFoodEffect()
        {
            if (m_ParticleInstanceManager)
            {
                m_ParticleInstanceManager.Stop(true);
            }
        }

        void ResincronyzationOnMainIdleAnimation()
        {
            m_Animator.Play(m_IdleHashFullPathPlayerPrefabForSync, 0, m_PlayerPrefabAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * m_IdleHandAnimSpeed);
        }
    }

    public enum HandsType
    {
        NONE = 0,
        LEFT = 1, RIGHT = 2
    }
}