using Dialog;
using UnityEngine;

public class DialogInfosTriggerZone : TriggerEffectZone
{
    public GameObject triggerZone;
    public bool secondHandActivation;
    public int index = 0;
    [HideInInspector] public DialogInfos m_CurrentDialogInfos;
    public GameEventScriptableObject m_DialogInfosBindingEvent;
    public GameEventScriptableObject m_CallDialogEvent;

    private void Start()
    {
        if (secondHandActivation) {
            triggerZone.SetActive(false);
        }
    }

    protected override void TriggerFunc(Collider other)
    {
        if (m_IsDestroyed) return;

        m_CallDialogEvent.TriggerEvent(m_CurrentDialogInfos);

        if (secondHandActivation)
        {
            triggerZone.SetActive(true);
        }
    }
}
