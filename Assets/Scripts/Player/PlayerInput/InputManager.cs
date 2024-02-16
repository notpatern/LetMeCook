using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Input
{

    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        private InputAction _wasd;
        private InputAction _jump;
        private InputAction _dash;
        private InputAction _interact;
        private InputAction _leftHand;
        private InputAction _rightHand;

        UnityEvent<Vector2> m_OnWASDInput;
        UnityEvent m_OnJumpInput;
        UnityEvent m_OnDashinput;
        UnityEvent m_OnInteractInput;
        UnityEvent<HandSystem.HandsType> m_OnLeftHandInput;
        UnityEvent<HandSystem.HandsType> m_OnRightHandInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            _wasd = _playerInput.Player.WASD;
            _wasd.Enable();
            _wasd.performed += WasdMovement;

            _jump = _playerInput.Player.Jump;
            _jump.Enable();
            _jump.performed += Jump;

            _dash = _playerInput.Player.Jump;
            _dash.Enable();
            _dash.performed += Dash;

            _interact = _playerInput.Player.Interact;
            _interact.Enable();
            _interact.performed += Interact;

            _leftHand = _playerInput.Player.LeftHand;
            _leftHand.Enable();
            _leftHand.performed += LeftHand;

            _rightHand = _playerInput.Player.RightHand;
            _rightHand.Enable();
            _rightHand.performed += RightHand;
        }

        private void OnDisable()
        {
            _wasd.Disable();
            _jump.Disable();
            _dash.Disable();
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

        public void BindLeftHand(UnityAction<HandSystem.HandsType> action)
        {
            m_OnLeftHandInput.AddListener(action);
        }

        public void BindRightHand(UnityAction<HandSystem.HandsType> action)
        {
            m_OnRightHandInput.AddListener(action);
        }
    }

}