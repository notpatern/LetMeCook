using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using PlayerSystems.PlayerInput;

namespace Player.Interaction
{
    public class PlayerInteractionUI : MonoBehaviour
    {
        [SerializeField] TMP_Text intereactionText;
        [SerializeField] KeybindsData m_keybindsData;

        InputAction m_action;

        void Start()
        {
            m_action = InputManager.s_PlayerInput.asset.FindAction(m_keybindsData.inputActionReference.action.id);
        }

        public void SetActiveInteractionText(bool state)
        {
            intereactionText.gameObject.SetActive(state);
        }

        void UpdateInteractionText(string data)
        {
            intereactionText.text = "You can get " + data;
        }

        public void StartInteraction(bool state, string data)
        {
            SetActiveInteractionText(state);
            UpdateInteractionText(data);
        }
    }
}