using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player.Input
{
    public class InputActionDisplay : MonoBehaviour
    {
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private int bindingIndex;

        private InputAction action;

        private Button rebindButton;

        private void Awake()
        {
            rebindButton = GetComponentInChildren<Button>();
            rebindButton.onClick.AddListener(RebindAction);
        }

        private void OnEnable()
        {
            action = InputManager.s_PlayerInput.asset.FindAction(actionReference.action.id);

            SetButtonText();
        }

        private void SetButtonText()
        {
            rebindButton.GetComponentInChildren<TextMeshProUGUI>().text = action.GetBindingDisplayString(bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        }

        private void RebindAction()
        {
            rebindButton.GetComponentInChildren<TextMeshProUGUI>().text = "...";

            ControlsRemapping.SuccessfulRebinding += OnSuccessfulRebinding;

            bool isGamepad = action.bindings[bindingIndex].path.Contains("Gamepad");

            if (isGamepad)
                ControlsRemapping.RemapGamepadAction(action, bindingIndex);
            else
                ControlsRemapping.RemapKeyboardAction(action, bindingIndex);
        }

        private void OnSuccessfulRebinding(InputAction action)
        {
            ControlsRemapping.SuccessfulRebinding -= OnSuccessfulRebinding;
            SetButtonText();
        }

        private void OnDestroy()
        {
            rebindButton.onClick.RemoveAllListeners();
        }
    }
}