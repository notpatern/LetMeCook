using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvent", menuName = "LetMeCook/GameEvent")]
public class GameEventScriptableObject : ScriptableObject
{
    UnityEvent<object> m_GameEvent;

    public void TriggerEvent(object args = null)
    {
        m_GameEvent.Invoke(args);
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
