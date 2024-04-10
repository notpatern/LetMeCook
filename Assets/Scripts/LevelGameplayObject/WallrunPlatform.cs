using UnityEngine;
using UnityEngine.Serialization;

public class WallrunPlatform : MonoBehaviour
{
    [SerializeField] GameEventScriptableObject m_GameEvent;
    [SerializeField] GameObject m_eyeLid;

    void Awake()
    {
        m_eyeLid.SetActive(true);
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
        m_eyeLid.SetActive(!(bool)args);
    }
}
