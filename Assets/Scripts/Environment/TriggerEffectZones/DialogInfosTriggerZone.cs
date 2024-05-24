using Dialog;
using UnityEngine;

public class DialogInfosTriggerZone : TriggerEffectZone
{
    public GameObject triggerZone;
    public bool secondHandActivation;
    public bool destroyOnTrigger;
    public int index = 0;
    [HideInInspector] public DialogInfos m_CurrentDialogInfos;
    public GameEventScriptableObject m_DialogInfosBindingEvent;
    public GameEventScriptableObject m_CallDialogEvent;

    bool isDestroyed;

    private void Start()
    {
        if (secondHandActivation) {
            triggerZone.SetActive(false);
        }
    }

    protected override void TriggerFunc(Collider other)
    {
        if (isDestroyed) return;

        m_CallDialogEvent.TriggerEvent(m_CurrentDialogInfos);

        if(destroyOnTrigger)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }

        if (secondHandActivation)
        {
            triggerZone.SetActive(true);
        }
    }
}
