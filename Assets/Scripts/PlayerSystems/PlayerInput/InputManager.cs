using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [HideInInspector] public InputAction wasd;
    private InputAction _jump;
    private InputAction _dash;
    private InputAction _interact;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        wasd = _playerInput.Player.WASD;
        wasd.Enable();

        _jump = _playerInput.Player.Jump;
        _jump.Enable();
        _jump.performed += Jump;

        _dash = _playerInput.Player.Jump;
        _dash.Enable();
        _dash.performed += Dash;

        _interact = _playerInput.Player.Interact;
        _interact.Enable();
        _interact.performed += Interact;
    }

    private void OnDisable()
    {
        wasd.Disable();
        _jump.Disable();
        _dash.Disable();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        
    }

    private void Dash(InputAction.CallbackContext context)
    {
        
    }

    private void Interact(InputAction.CallbackContext context)
    {
        
    }
}
