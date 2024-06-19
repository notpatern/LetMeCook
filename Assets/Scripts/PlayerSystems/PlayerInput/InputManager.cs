using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PlayerSystems.PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        public static global::PlayerInput s_PlayerInput;
        [HideInInspector] public InputAction wasd;
        UnityEvent<Vector2> m_OnWASDInput = new UnityEvent<Vector2>();
        UnityEvent m_OnJumpInput = new UnityEvent();
        UnityEvent m_OnDashinput = new UnityEvent();
        UnityEvent m_OnInteractInput = new UnityEvent();
        UnityEvent<Player.HandSystem.HandsType> m_OnLeftHandInput = new UnityEvent<Player.HandSystem.HandsType>();
        UnityEvent<Player.HandSystem.HandsType> m_OnRightHandInput = new UnityEvent<Player.HandSystem.HandsType>();
        UnityEvent m_MergeHandInput = new UnityEvent();
        UnityEvent m_TogglePauseMenu = new UnityEvent();
        [SerializeField] GameEventScriptableObject m_PlayerSetActiveInput;

        private bool _jumpHeld;

        private void Awake()
        {
            if (s_PlayerInput == null)
            {
                s_PlayerInput = new global::PlayerInput();
                m_PlayerSetActiveInput.BindEventAction(SetActiveInputActionBinding);
            }
        }

        private void OnEnable()
        {
            if(s_PlayerInput == null) return;

            s_PlayerInput.Enable();
            
            s_PlayerInput.Player.WASD.performed += WasdMovement;
            s_PlayerInput.Player.WASD.canceled += WasdMovement;
            s_PlayerInput.Player.Jump.performed += Jump;
            s_PlayerInput.Player.Jump.canceled += JumpReleased;
            s_PlayerInput.Player.Dash.performed += Dash;
            s_PlayerInput.Player.LeftHand.performed += LeftHand;
            s_PlayerInput.Player.RightHand.performed += RightHand;
            s_PlayerInput.Player.MergeHand.performed += MergeHandInput;
            s_PlayerInput.Player.TogglePauseMenu.performed += TogglePauseMenu;
        }

        void SetActiveInputActionBinding(object args)
        {
            Tuple<KeybindsData, bool> action = args as Tuple<KeybindsData, bool>;

            SetActiveInput(action.Item1, action.Item2);
        }

        void SetActiveInput(KeybindsData keybindsData, bool state)
        {
            foreach(InputAction input in s_PlayerInput)
            {
                if (input.id == keybindsData.inputActionReference.action.id)
                {
                    if (state)
                    {
                        input.Enable();
                    }
                    else
                    {
                        input.Disable();
                    }

                    return;
                }
            }
        }

        private void OnDisable()
        {
            if(s_PlayerInput == null) return;
            
            s_PlayerInput.Disable();

            s_PlayerInput.Player.WASD.performed -= WasdMovement;
            s_PlayerInput.Player.WASD.canceled -= WasdMovement;
            s_PlayerInput.Player.Jump.performed -= Jump;
            s_PlayerInput.Player.Jump.canceled -= JumpReleased;
            s_PlayerInput.Player.Dash.performed -= Dash;
            s_PlayerInput.Player.LeftHand.performed -= LeftHand;
            s_PlayerInput.Player.RightHand.performed -= RightHand;
            s_PlayerInput.Player.MergeHand.performed -= MergeHandInput;
            s_PlayerInput.Player.TogglePauseMenu.performed -= TogglePauseMenu;
        }

        private void WasdMovement(InputAction.CallbackContext context)
        {
            m_OnWASDInput.Invoke(context.ReadValue<Vector2>());
        }

        private void Jump(InputAction.CallbackContext context)
        {
            _jumpHeld = true;
            m_OnJumpInput.Invoke();
        }

        private void JumpReleased(InputAction.CallbackContext context)
        {
            _jumpHeld = false;
        }

        public bool GetJumpHeld()
        {
            return _jumpHeld;
        }

        private void Dash(InputAction.CallbackContext context)
        {
            m_OnDashinput.Invoke();
        }

        private void LeftHand(InputAction.CallbackContext context)
        {
            m_OnLeftHandInput.Invoke(Player.HandSystem.HandsType.LEFT);
        }

        private void RightHand(InputAction.CallbackContext context)
        {
            m_OnRightHandInput.Invoke(Player.HandSystem.HandsType.RIGHT);
        }

        private void MergeHandInput(InputAction.CallbackContext context)
        {
            m_MergeHandInput.Invoke();
        }

        private void TogglePauseMenu(InputAction.CallbackContext context)
        {
            m_TogglePauseMenu.Invoke();
        }


        //Bind event---------------------------
        public void BindWasdMovement(UnityAction<Vector2> action)
        {
            m_OnWASDInput.AddListener(action);
        }

        public void BindJump(UnityAction action)
        {
            m_OnJumpInput.AddListener(action);
        }

        public void BindDash(UnityAction action)
        {
            m_OnDashinput.AddListener(action);
        }

        public void BindInteract(UnityAction action)
        {
            m_OnInteractInput.AddListener(action);
        }

        public void BindHandAction(UnityAction<Player.HandSystem.HandsType> action)
        {
            m_OnLeftHandInput.AddListener(action);
            m_OnRightHandInput.AddListener(action);
        }

        public void BindMergeHandInput(UnityAction action)
        {
            m_MergeHandInput.AddListener(action);
        }

        public void BindTogglePauseMenu(UnityAction action)
        {
            m_TogglePauseMenu.AddListener(action);
        }
    }
}