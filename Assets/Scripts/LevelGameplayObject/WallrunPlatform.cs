using UnityEngine;

public class WallrunPlatform : MonoBehaviour
{
    [SerializeField] GameEventScriptableObject m_GameEvent;
    [SerializeField] GameObject m_LigthsEffect;

    void Awake()
    {
        m_LigthsEffect.SetActive(false);
    }

    void OnEnable()
    {
        m_GameEvent.BindEventAction(ActiveLight);
    }

    void OnDisable()
    {
        m_GameEvent.UnbindEventAction(ActiveLight);
    }

    void ActiveLight(object args)
    {
        m_LigthsEffect.SetActive((bool)args);
    }
}
