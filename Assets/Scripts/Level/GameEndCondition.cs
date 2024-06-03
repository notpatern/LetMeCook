using System;
using UnityEngine.Events;
using UI;

namespace Manager
{
    public interface GameEndCondition
    {
        float m_Timer { get; set; }
        public void InitGameEndCondition(float time, EndConditionUI endConditionUI);
        public void Update(float dt);
        public void BindOnEndCondition(UnityAction action);
    }

    [Serializable]
    public class DefaultGameEndCondition : GameEndCondition
    {
        public UnityEvent UnityEvent = new UnityEvent();
        public float m_Timer { get; set; }
        bool m_IsFinish = false;
        EndConditionUI m_EndConditionUI;

        public void InitGameEndCondition(float time, EndConditionUI endConditionUI)
        {
            m_Timer = time;
            m_EndConditionUI = endConditionUI;
        }

        public void Update(float dt)
        {
            if(m_IsFinish)
            {
                return;
            }

            m_Timer -= dt;

            UpdateUI();

            if (m_Timer <= 0f)
            {
                m_Timer = 0f;
                m_IsFinish = true;
                UnityEvent.Invoke();
            }
        }

        void UpdateUI()
        {
            int minutes = (int)(m_Timer / 60f);
            int seconds = (int)(m_Timer % 60f);

            string color = minutes == 0 && seconds <= 10 ? "#FF6537" : "#FFFFFF";

            m_EndConditionUI.UpdateText(minutes.ToString("0") + ":" + seconds.ToString("00"), color);
        }

        public void BindOnEndCondition(UnityAction action)
        {
            UnityEvent.AddListener(action);
        }
    }
}