using PlayerSystems.PlayerBase;
using System;
using UnityEngine;

public class SetActivePlayerInputTriggerZone : TriggerEffectZone
{
    [SerializeField] GameEventScriptableObject m_PlayerSetActiveInput;
    [SerializeField] KeybindsData m_KeybindsData;
    [SerializeField] TutorialPlayer m_TutorialPlayer;
    [SerializeField] bool m_State;

    protected override void TriggerFunc(Collider other)
    {
        if (m_IsDestroyed) return;

        Tuple<KeybindsData, bool> action = new (m_KeybindsData, m_State);
        m_PlayerSetActiveInput.TriggerEvent(action);

        if (m_TutorialPlayer)
        {
            m_TutorialPlayer.SetActiveRightHand(m_State);
        }
    }
}
