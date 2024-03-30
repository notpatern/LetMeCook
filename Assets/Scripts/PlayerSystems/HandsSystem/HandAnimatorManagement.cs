using UnityEngine;
using UnityEngine.Events;

namespace Player.HandSystem
{

    public class HandAnimatorManagement : MonoBehaviour
    {
        UnityEvent m_ResincronyzeIdleAction = new UnityEvent();
        [SerializeField] GameObject m_MagicalCircleParticlePrefab;
        GameObject m_ParticleInstance;
        [SerializeField] Transform m_HandTransform;

        public void BindResincronyzationOnMainIdle(UnityAction action)
        {
            m_ResincronyzeIdleAction.AddListener(action);
        }

        public void AskResincronyzeAnimatorIdle()
        {
            m_ResincronyzeIdleAction.Invoke();
        }

        public void SpawnMagicalParticle()
        {
            m_ParticleInstance = Instantiate(m_MagicalCircleParticlePrefab, m_HandTransform);
            m_ParticleInstance.transform.SetParent(null);
            m_ParticleInstance.transform.localScale = Vector3.one;
        }

        public void StopMagicalParticle()
        {
            if (m_ParticleInstance)
            {
                Destroy(m_ParticleInstance);
            }

        }
    }

}