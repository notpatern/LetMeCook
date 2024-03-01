using Player.Input;
using PlayerSystems.Input;
using UnityEngine;

namespace ControlOptions
{
    public class ControlOptionsManagement
    {
        public static ControlOptionsManagement s_Instance;

        BoolTable canMove;
        float mouseSensitivity = 1.0f;

        public static void LoadControlOptionsManagement()
        {
            if(s_Instance == null)
            {
                s_Instance = new ControlOptionsManagement();
                ControlsRemapping.LoadMap();

                s_Instance.mouseSensitivity = PlayerPrefs.GetFloat("control_options_mouse_sensitivity", 1.0f);
            }
        }

        public void SetMouseSensitivity(float newValue)
        {
            mouseSensitivity = newValue;
            PlayerPrefs.SetFloat("control_options_mouse_sensitivity", mouseSensitivity);
        }

        public float GetMouseSensitivity()
        {
            return mouseSensitivity;
        }

        public void UpdateIsControlsActivated(bool value)
        {
            canMove.Value = value;

            if(canMove == true)
                InputManager.s_PlayerInput.Disable();
            else
            {
                InputManager.s_PlayerInput.Enable();
            }
        }

    }
}