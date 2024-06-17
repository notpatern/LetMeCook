using UnityEngine;

public abstract class TriggerEffectZone : MonoBehaviour
{
    public LayerMask triggerableLayers;
    public bool m_DestroyOnEnter;
    protected bool m_IsDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerableLayers == (triggerableLayers | (1 << other.gameObject.layer)))
        {
            TriggerFunc(other);

            if(m_DestroyOnEnter)
            {
                m_IsDestroyed = true;
                Destroy(gameObject);
            }
        }
    }

    protected abstract void TriggerFunc(Collider other);
}
