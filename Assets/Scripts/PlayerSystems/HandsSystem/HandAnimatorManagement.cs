using UnityEngine;
using UnityEngine.Events;

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
        GameObject m_ParticleCircleInstance;
        GameObject m_ParticleFogInstance;
        [SerializeField] Transform m_HandTransform;

        void Awake()
        {
            m_GameEventCanSpawnMagicalFogForMerge.BindEventAction(CanSpawnFogParticle);
        }

        public void BindRemoveFoodFromHand(UnityAction action)
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

        public void AskRemoveFoodFromHand()
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
            SpawnPartcile(false, out m_ParticleCircleInstance, m_MagicalCircleParticlePrefab);
        }

        public void SpawnFogParticle()
        {
            if (canSpawnFogParticle)
            {
                SpawnPartcile(false, out m_ParticleFogInstance, m_MagicalFogParticlePrefab);
            }
        }

        public void SpawnCrunchFoodFromHandsParticles()
        {
            GameObject particle;
            SpawnPartcile(true, out particle, m_CrunchFoodParticlesPrefab);
        }

        void CanSpawnFogParticle(object isPossible)
        {
            canSpawnFogParticle = (bool)isPossible;
        }

        void SpawnPartcile(bool parentToHand, out GameObject savedInstance, GameObject particlePrefab)
        {
            savedInstance = Instantiate(particlePrefab, m_HandTransform);

            if (!parentToHand)
            {
                savedInstance.transform.SetParent(null);
                savedInstance.transform.localScale = Vector3.one;
            }
        }

        public void StopMagicalCircleParticle()
        {
            StopParticle(m_ParticleCircleInstance);

        }

        public void StopMagicalFogParticle()
        {
            StopParticle(m_ParticleFogInstance);
        }

        void StopParticle(GameObject particleInstance)
        {
            if (particleInstance)
            {
                Destroy(particleInstance);
            }
        }
    }
}