using UnityEngine;

public class SetActivePlayerInputTriggerZone : TriggerEffectZone
{
    [SerializeField] GameEventScriptableObject m_PlayerSetActiveInput;
    [SerializeField] KeybindsData m_KeybindsData;
    [SerializeField] bool state;

    protected override void TriggerFunc(Collider other)
    {
        if (m_IsDestroyed) return;
        
        m_PlayerSetActiveInput.TriggerEvent((m_KeybindsData, state));
    }
}
