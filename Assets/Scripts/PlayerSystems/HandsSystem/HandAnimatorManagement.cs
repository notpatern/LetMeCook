using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Player.HandSystem
{
    public class HandAnimatorManagement : MonoBehaviour
    {
        UnityEvent m_ResincronyzeIdle = new UnityEvent();
        UnityEvent m_StopGrabParticle = new UnityEvent();
        UnityEvent m_RemoveFoodFromHand = new UnityEvent();
        [SerializeField] GameObject m_MagicalCircleParticlePrefab;
        [SerializeField] GameObject m_MagicalFogParticlePrefab;
        [SerializeField] GameObject m_CrunchFoodParticlesPrefab;
        bool canSpawnFogParticle = true;
        [SerializeField] GameEventScriptableObject m_GameEventCanSpawnMagicalFogForMerge;
        [SerializeField] Transform m_HandTransform;

        void Awake()
        {
            m_GameEventCanSpawnMagicalFogForMerge.BindEventAction(CanSpawnFogParticle);
        }

        public void BindCrushFoodFromHand(UnityAction action)
        {
            m_RemoveFoodFromHand.AddListener(action);
        }

        public void BindResincronyzationOnMainIdle(UnityAction action)
        {
            m_ResincronyzeIdle.AddListener(action);
        }

        public void BindStopGrabParticle(UnityAction action)
        {
            m_StopGrabParticle.AddListener(action);
        }

        public void AskCrushFoodFromHand()
        {
            m_RemoveFoodFromHand.Invoke();
        }

        public void AskResincronyzeAnimatorIdle()
        {
            m_ResincronyzeIdle.Invoke();
        }

        public void AskStopGrabParticle()
        {
            m_StopGrabParticle.Invoke();
        }

        public void SpawnMagicalCircleParticle()
        {
            SpawnPartcile(false, m_MagicalCircleParticlePrefab);
        }

        public void SpawnFogParticle()
        {
            if (canSpawnFogParticle)
            {
                SpawnPartcile(false, m_MagicalFogParticlePrefab);
            }
        }

        public void SpawnCrunchFoodFromHandsParticles()
        {
            SpawnPartcile(true, m_CrunchFoodParticlesPrefab);
        }

        void CanSpawnFogParticle(object isPossible)
        {
            canSpawnFogParticle = (bool)isPossible;
        }

        void SpawnPartcile(bool parentToHand, GameObject particlePrefab)
        {
            GameObject go = Instantiate(particlePrefab, m_HandTransform);

            if (!parentToHand)
            {
                go.transform.SetParent(null);
                go.transform.localScale = Vector3.one;
            }
        }
    }
}