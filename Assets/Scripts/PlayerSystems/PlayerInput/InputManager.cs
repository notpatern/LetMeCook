using Player.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [HideInInspector] public InputAction wasd;
        private InputAction _jump;
        private InputAction _dash;
        private InputAction _interact;
        UnityEvent<Vector2> m_OnWASDInput;
        UnityEvent m_OnJumpInput;
        UnityEvent m_OnDashinput;
        UnityEvent m_OnInteractInput;
        UnityEvent<HandSystem.HandsType> m_OnLeftHandInput;
        UnityEvent<HandSystem.HandsType> m_OnRightHandInput;
        UnityEvent m_TogglePauseMenu;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            m_OnWASDInput = new UnityEvent<Vector2>();
            m_OnJumpInput = new UnityEvent();
            m_OnDashinput = new UnityEvent();
            m_OnInteractInput = new UnityEvent();
            m_OnLeftHandInput = new UnityEvent<HandSystem.HandsType>();
            m_OnRightHandInput = new UnityEvent<HandSystem.HandsType>();
            m_TogglePauseMenu = new UnityEvent();
        }

        private void OnEnable()
        {
            _playerInput.Enable();

            _playerInput.Player.WASD.performed += WasdMovement;
            _playerInput.Player.WASD.canceled += WasdMovement;
            _playerInput.Player.Jump.performed += Jump;
            _playerInput.Player.Jump.performed += Dash;
            _playerInput.Player.Interact.performed += Interact;
            _playerInput.Player.LeftHand.performed += LeftHand;
            _playerInput.Player.RightHand.performed += RightHand;
            _playerInput.Player.TogglePauseMenu.performed += TogglePauseMenu;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        private void WasdMovement(InputAction.CallbackContext context)
        {
            m_OnWASDInput.Invoke(context.ReadValue<Vector2>());
        }

        private void Jump(InputAction.CallbackContext context)
        {
            m_OnJumpInput.Invoke();
        }

        private void Dash(InputAction.CallbackContext context)
        {
            m_OnDashinput.Invoke();
        }

        private void Interact(InputAction.CallbackContext context)
        {
            m_OnInteractInput.Invoke();
        }

        private void LeftHand(InputAction.CallbackContext context)
        {
            m_OnLeftHandInput.Invoke(HandSystem.HandsType.LEFT);
        }

        private void RightHand(InputAction.CallbackContext context)
        {
            m_OnRightHandInput.Invoke(HandSystem.HandsType.RIGHT);
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

        public void BindHandAction(UnityAction<HandSystem.HandsType> action)
        {
            m_OnLeftHandInput.AddListener(action);
            m_OnRightHandInput.AddListener(action);
        }

        public void BindTogglePauseMenu(UnityAction action)
        {
            m_TogglePauseMenu.AddListener(action);
        }
    }
}