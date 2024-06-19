using UnityEngine;

public class SetActiveGameObjectTriggerZone : TriggerEffectZone
{
    [SerializeField] GameObject m_ObjectToSetActive;
    [SerializeField] bool m_State;
    [SerializeField] bool m_DefaultState;

    private void Start()
    {
        m_ObjectToSetActive.SetActive(m_DefaultState);
    }

    protected override void TriggerFunc(Collider other)
    {
        m_ObjectToSetActive.SetActive(m_State);
    }
}
