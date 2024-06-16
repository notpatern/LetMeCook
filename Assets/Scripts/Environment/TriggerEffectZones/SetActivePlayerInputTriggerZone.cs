using System;
using UnityEngine;

public class SetActivePlayerInputTriggerZone : TriggerEffectZone
{
    [SerializeField] GameEventScriptableObject m_PlayerSetActiveInput;
    [SerializeField] KeybindsData m_KeybindsData;
    [SerializeField] bool state;

    protected override void TriggerFunc(Collider other)
    {
        if (m_IsDestroyed) return;

        Tuple<KeybindsData, bool> action = new (m_KeybindsData, state);
        m_PlayerSetActiveInput.TriggerEvent(action);
    }
}
