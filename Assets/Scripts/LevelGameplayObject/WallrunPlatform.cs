
using UnityEngine;

public class WallrunPlatform : MonoBehaviour
{
    [SerializeField] GameEventScriptableObject m_GameEvent;
    [SerializeField] GameObject m_Energy;

    bool isEnergyActive;

    void Awake()
    {
        ActiveLight(false);
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
        isEnergyActive = (bool)args;
        m_Energy.SetActive((bool)args);
    }
}
