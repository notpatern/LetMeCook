using Player.Input;
using PlayerSystems.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlOptions
{
    public class ControlOptionsManagement
    {
        public static ControlOptionsManagement s_Instance;

        BoolTable canMove;
        float mouseSensitivity = 1.0f;

        public ControlOptionsManagement()
        {
            canMove = new BoolTable();
            mouseSensitivity = PlayerPrefs.GetFloat("control_options_mouse_sensitivity", 1.0f);
        }

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
            PlayerPrefs.SetFloat("control_options_mouse_sensitivity", mouseSensitivity);
        }

        public float GetMouseSensitivity()
        {
            return mouseSensitivity;
        }

        public void UpdateIsMainControlsActivated(bool value)
        {
            canMove.Value = value;

            if(canMove.Value == false)
            {
                DisableMainPlayerInputs();
            }
            else
            {
                EnableMainPlayerInputs();
            }
        }

        public void EnableMainPlayerInputs()
        {
            PlayerInput.PlayerActions playerInput = InputManager.s_PlayerInput.Player;
            playerInput.Enable();
        }

        public void DisableMainPlayerInputs()
        {
            PlayerInput.PlayerActions playerInput = InputManager.s_PlayerInput.Player;
            playerInput.Disable();
            foreach(InputAction action in playerInput.Get())
            {
                action.Disable();
            }
            playerInput.TogglePauseMenu.Enable();
        }

    }
}