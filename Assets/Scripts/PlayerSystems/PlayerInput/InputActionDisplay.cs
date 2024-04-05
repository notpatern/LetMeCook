using PlayerSystems.PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player.Input
{
    public class InputActionDisplay : MonoBehaviour
    {
        KeybindsData m_keybindsData;

        InputAction m_action;

        Button m_rebindButton;
        [SerializeField] TMP_Text nameText;

        public void LoadInput(KeybindsData keybindsData)
        {
            m_keybindsData = keybindsData;

            nameText.text = keybindsData.DisplayedKeyName;

            m_rebindButton = GetComponentInChildren<Button>();
            m_rebindButton.onClick.AddListener(RebindAction);

            m_action = InputManager.s_PlayerInput.asset.FindAction(m_keybindsData.inputActionReference.action.id);

            SetButtonText();
        }

        void SetButtonText()
        {
            m_rebindButton.GetComponentInChildren<TextMeshProUGUI>().text = m_action.GetBindingDisplayString(m_keybindsData.bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        }

        void RebindAction()
        {
            m_rebindButton.GetComponentInChildren<TextMeshProUGUI>().text = "...";

            ControlsRemapping.SuccessfulRebinding += OnSuccessfulRebinding;

            ControlsRemapping.RemapKeyboardMouseAction(m_action, m_keybindsData.bindingIndex);
        }

        void OnSuccessfulRebinding(InputAction action)
        {
            ControlsRemapping.SuccessfulRebinding -= OnSuccessfulRebinding;
            SetButtonText();
        }

        void OnDestroy()
        {
            m_rebindButton.onClick.RemoveAllListeners();
        }
    }
}