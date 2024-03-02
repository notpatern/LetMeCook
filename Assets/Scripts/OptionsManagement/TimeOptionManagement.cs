using UnityEngine;

namespace TimeOption
{
    public class TimeOptionManagement
    {
        public static TimeOptionManagement s_Instance;

        BoolTable m_IsTimeActivated;

        public TimeOptionManagement()
        {
            m_IsTimeActivated = new BoolTable();
        }

        public static void LoadTimeOptionsManagement()
        {
            if(s_Instance == null)
            {
                s_Instance = new TimeOptionManagement();
            }
        }

        public void SetActiveTime(bool state)
        {
            m_IsTimeActivated.Value = state;

            Time.timeScale = m_IsTimeActivated.Value ? 1.0f : 0.0f;

            ControlOptions.ControlOptionsManagement.s_Instance.UpdateIsMainControlsActivated(state);
        }
    }
}