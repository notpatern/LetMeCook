using UnityEngine;
using UnityEngine.Events;

namespace Player.HandSystem
{
    public class HandAnimatorManagement : MonoBehaviour
    {
        UnityEvent m_ResincronyzeIdle = new UnityEvent();
        UnityEvent m_StopGrabParticle = new UnityEvent();
        [SerializeField] GameObject m_MagicalCircleParticlePrefab;
        [SerializeField] GameObject m_MagicalFogParticlePrefab;
        GameObject m_ParticleCircleInstance;
        GameObject m_ParticleFogInstance;
        [SerializeField] Transform m_HandTransform;

        public void BindResincronyzationOnMainIdle(UnityAction action)
        {
            m_ResincronyzeIdle.AddListener(action);
        }

        public void BindStopGrabParticle(UnityAction action)
        {
            m_StopGrabParticle.AddListener(action);
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
            SpawnPartcile(true, out m_ParticleCircleInstance, m_MagicalCircleParticlePrefab);
        }

        public void SpawnFogParticle()
        {
            SpawnPartcile(true, out m_ParticleFogInstance, m_MagicalFogParticlePrefab);
        }

        void SpawnPartcile(bool parentToHand, out GameObject savedInstance, GameObject particlePrefab)
        {
            savedInstance = Instantiate(particlePrefab, m_HandTransform);

            if (parentToHand)
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