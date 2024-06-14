 using UnityEngine;
using TMPro;
using System.Collections;

namespace UI
{
    public class EndConditionUI : MonoBehaviour
    {
        [SerializeField] TMP_Text timerText;
        [SerializeField] TMP_Text timeRemainingWarning;
        [SerializeField] GameObject warningGoInHand;

        [HideInInspector] public PlayerWarningText playerWarningText;

        void Start()
        {
            timerText.text = "";
            timeRemainingWarning.text = "";
            warningGoInHand.SetActive(false);
        }

        public void Init(Transform timerParent, PlayerWarningText newPlayerWarningText)
        {
            playerWarningText = newPlayerWarningText;

            timerText.text = "";
            timeRemainingWarning.text = "";
            warningGoInHand.SetActive(false);
        }

        public void UpdateText(string data, string hexColor)
        {
            timerText.text = $"<color={hexColor}>{data}</color>";
        }

        public void ActiveWarningPanel(float warningPanelMiddleScreenDuration)
        {
            warningGoInHand.SetActive(true);
            playerWarningText.ActiveWarningPanel(warningPanelMiddleScreenDuration);
        }

        public void ShowRemainingTimeWarning(string remainingTime)
        {
            timeRemainingWarning.text = remainingTime + "s";
        }
    }

}