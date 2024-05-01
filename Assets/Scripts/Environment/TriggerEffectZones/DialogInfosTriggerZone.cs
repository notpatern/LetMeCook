using Dialog;
using UnityEngine;

public class DialogInfosTriggerZone : TriggerEffectZone
{
    public bool destroyOnTrigger;
    [HideInInspector] public DialogInfos m_CurrentDialogInfos;
    public GameEventScriptableObject m_DialogInfosBindingEvent;
    public GameEventScriptableObject m_CallDialogEvent;

    bool isDestroyed;

    protected override void TriggerFunc(Collider other)
    {
        if (isDestroyed) return;

        Debug.Log(other.gameObject.name);
        m_CallDialogEvent.TriggerEvent(m_CurrentDialogInfos);

        if(destroyOnTrigger)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
