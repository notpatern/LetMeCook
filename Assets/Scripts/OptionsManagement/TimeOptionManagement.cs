using Audio;
using UnityEngine;

namespace TimeOption
{
    public static class TimeOptionManagement
    {
        static BoolTable m_IsTimeActivated;

        public static void LoadTimeOptionsManagement()
        {
            m_IsTimeActivated = new BoolTable();
        }

        public static void SetActiveTime(bool state)
        {
            m_IsTimeActivated.Value = state;

            Time.timeScale = m_IsTimeActivated.Value ? 1.0f : 0.0f;

            AudioManager.s_Instance.SetPause(!m_IsTimeActivated.Value);
            //ControlOptions.ControlOptionsManagement.s_Instance.UpdateIsMainControlsActivated(state);
        }
    }
}