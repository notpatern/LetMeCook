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
            playerInput.WASD.Enable();
            playerInput.Jump.Enable();
            playerInput.Dash.Enable();
            playerInput.Interact.Enable();
            playerInput.LeftHand.Enable();
            playerInput.RightHand.Enable();
            playerInput.MergeHand.Enable();
        }

        public void DisableMainPlayerInputs()
        {
            PlayerInput.PlayerActions playerInput = InputManager.s_PlayerInput.Player;
            playerInput.WASD.Disable();
            playerInput.Jump.Disable();
            playerInput.Dash.Disable();
            playerInput.Interact.Disable();
            playerInput.LeftHand.Disable();
            playerInput.RightHand.Disable();
            playerInput.MergeHand.Disable();
        }

    }
}