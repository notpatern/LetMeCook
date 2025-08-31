using UnityEngine;
public class SetPlayerCanThrow : TriggerEffectZone
{
    public bool state = true;
    public GameEventScriptableObject canThrowEvent;

    protected override void TriggerFunc(Collider other)
    {
        if (m_IsDestroyed) return;

        canThrowEvent.TriggerEvent(state);
    }
}
