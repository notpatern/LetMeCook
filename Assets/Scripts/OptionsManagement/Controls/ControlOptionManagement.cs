using Player.Input;

namespace ControlOptions
{
    public class ControlOptionsManagement
    {
        public static ControlOptionsManagement s_Instance;

        //Mouse
        float mouseSensitivity = 0.0f;

        public static void LoadControlOptionsManagement()
        {
            if(s_Instance == null)
            {
                s_Instance = new ControlOptionsManagement();
                ControlsRemapping.LoadMap();
            }
        }

        public void SetMouseSensitivity(float newValue)
        {
            mouseSensitivity = newValue;
        }
    }
}