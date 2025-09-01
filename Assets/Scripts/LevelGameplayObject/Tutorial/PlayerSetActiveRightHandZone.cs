using UnityEngine;

public class PlayerSetActiveRightHandZone : TriggerEffectZone
{
    public bool state = true;
    public GameEventScriptableObject setActiveRightHHand;

    protected override void TriggerFunc(Collider other)
    {
        if (m_IsDestroyed) return;

        setActiveRightHHand.TriggerEvent(state);
    }
}
