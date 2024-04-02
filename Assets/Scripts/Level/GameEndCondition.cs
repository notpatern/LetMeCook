using System;
using UnityEngine.Events;

namespace Manager
{
    public interface GameEndCondition
    {
        float m_Timer { get; set; }
        public void InitGameEndCondition(float time);
        public void Update(float dt);
        public void BindOnEndCondition(UnityAction action);
    }

    [Serializable]
    public class DefaultGameEndCondition : GameEndCondition
    {
        public UnityEvent UnityEvent = new UnityEvent();
        public float m_Timer { get; set; }

        public void InitGameEndCondition(float time)
        {
            m_Timer = time;
        }

        public void Update(float dt)
        {
            m_Timer -= dt;

            if(m_Timer <= 0f)
            {
                m_Timer = 0f;
                UnityEvent.Invoke();
            }
        }

        public void BindOnEndCondition(UnityAction action)
        {
            UnityEvent.AddListener(action);
        }
    }
}