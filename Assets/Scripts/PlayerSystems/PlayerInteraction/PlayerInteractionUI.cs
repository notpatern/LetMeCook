using UnityEngine;
using TMPro;

namespace Player.Interaction
{
    public class PlayerInteractionUI : MonoBehaviour
    {
        [SerializeField] TMP_Text intereactionText;

        public void SetActiveInteractionText(bool state)
        {
            intereactionText.gameObject.SetActive(state);
        }

        void UpdateInteractionText(string data)
        {
            intereactionText.text = "Press [F] to interact with " + data;
        }

        public void StartInteraction(bool state, string data)
        {
            SetActiveInteractionText(state);
            UpdateInteractionText(data);
        }
    }
}