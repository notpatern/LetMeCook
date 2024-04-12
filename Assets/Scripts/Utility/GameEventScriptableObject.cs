using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvent", menuName = "LetMeCook/GameEvent")]
public class GameEventScriptableObject : ScriptableObject
{
    UnityEvent<object> m_GameEvent = new UnityEvent<object>();
    [SerializeField] bool m_ClearAfterTriggered = false;

    public void TriggerEvent(object args = null)
    {
        m_GameEvent.Invoke(args);
        
        if(m_ClearAfterTriggered)
        {
            m_GameEvent.RemoveAllListeners();
        }
    }

    public void BindEventAction(UnityAction<object> action)
    {
        m_GameEvent.AddListener(action);
    }

    public void UnbindEventAction(UnityAction<object> action)
    {
        m_GameEvent.RemoveListener(action);
    }
}
