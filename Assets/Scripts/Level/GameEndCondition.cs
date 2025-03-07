using System;
using UnityEngine.Events;
using UI;
using UnityEngine;
using Audio;
using FMOD.Studio;

namespace Manager
{
    public interface GameEndCondition
    {
        float m_Timer { get; set; }
        public void InitGameEndCondition(float time, EndConditionUI endConditionUI, Transform timerParent, PlayerWarningText playerWarningText);
        public void Update(float dt);
        public void BindOnEndCondition(UnityAction action);
    }

    [Serializable]
    public class DefaultGameEndCondition : GameEndCondition
    {
        public UnityEvent m_OnEndConditionEvent = new UnityEvent();
        public float m_Timer { get; set; }
        bool m_IsFinish = false;
        EndConditionUI m_EndConditionUI;

        bool m_IsRemainingTimeWarningShowed = false;
        [SerializeField] float m_TimeRemainingWarningSeconds = 10f;
        float m_TimerWarningDuration = 3f;
        EventInstance warningRemainingTimeTicTac;

        public void InitGameEndCondition(float time, EndConditionUI endConditionUI, Transform timerParent, PlayerWarningText playerWarningText)
        {
            m_Timer = time;
            m_EndConditionUI = endConditionUI;
            endConditionUI.Init(timerParent, playerWarningText);

            m_OnEndConditionEvent.AddListener(() => { warningRemainingTimeTicTac.stop(STOP_MODE.ALLOWFADEOUT); });
        }

        public void Update(float dt)
        {
            if (!m_IsFinish && m_Timer <= 0f)
            {
                m_Timer = 0f;
                m_IsFinish = true;
                m_OnEndConditionEvent.Invoke();
            }

            if (m_IsFinish)
            {
                return;
            }

            m_Timer -= dt;

            UpdateUI();
        }

        void UpdateUI()
        {
            int minutes = (int)(m_Timer / 60f);
            int seconds = (int)(m_Timer % 60f);

            bool warningRemainingTime = minutes <= 0 && seconds <= m_TimeRemainingWarningSeconds;

            string color = warningRemainingTime ? "#FF6537" : "#FFFFFF";
            string content;
            if (warningRemainingTime)
            {
                int miliseconds = Mathf.FloorToInt((m_Timer % 1f) * 100);
                content = seconds.ToString("0") + ":" + miliseconds.ToString("00");
                m_EndConditionUI.ShowRemainingTimeWarning(content);

                if (!m_IsRemainingTimeWarningShowed)
                {
                    m_EndConditionUI.ActiveWarningPanel(m_TimerWarningDuration);
                    m_IsRemainingTimeWarningShowed = true;
                    warningRemainingTimeTicTac = AudioManager.s_Instance.CreateInstance(AudioManager.s_Instance.m_AudioSoundData.m_LevelTimeRemainingWarning);
                    warningRemainingTimeTicTac.start();
                }
            }
            else
            {
                content = minutes.ToString("0") + ":" + seconds.ToString("00");
            }

            m_EndConditionUI.UpdateText(content, color);
        }

        public void BindOnEndCondition(UnityAction action)
        {
            m_OnEndConditionEvent.AddListener(action);
        }
    }
}