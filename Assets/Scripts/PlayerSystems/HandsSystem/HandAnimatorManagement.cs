using UnityEngine;
using UnityEngine.Events;

public class HandAnimatorManagement : MonoBehaviour
{
    UnityEvent m_ResincronyzeIdleAction = new UnityEvent();
    [SerializeField] GameObject m_MagicalCircleParticlePrefab;
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
        Destroy(Instantiate(m_MagicalCircleParticlePrefab, m_HandTransform), 1f);
    }
}
