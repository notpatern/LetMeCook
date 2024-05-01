using UnityEngine;

public abstract class TriggerEffectZone : MonoBehaviour
{
    public LayerMask triggerableLayers;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerableLayers == (triggerableLayers | (1 << other.gameObject.layer)))
        {
            TriggerFunc(other);
        }
    }

    protected abstract void TriggerFunc(Collider other);
}
