 using UnityEngine;
using TMPro;
using System.Collections;

namespace UI
{
    public class EndConditionUI : MonoBehaviour
    {
        [SerializeField] TMP_Text timerText;
        [SerializeField] TMP_Text timeRemainingWarning;
        [SerializeField] GameObject warningGoInWorld;

        [HideInInspector] public PlayerWarningText playerWarningText;

        void Start()
        {
            timerText.text = "";
            timeRemainingWarning.text = "";
            warningGoInWorld.SetActive(false);
        }

        public void Init(Transform timerParent, PlayerWarningText newPlayerWarningText)
        {
            playerWarningText = newPlayerWarningText;

            timerText.text = "";
            timeRemainingWarning.text = "";
            warningGoInWorld.SetActive(false);
        }

        public void UpdateText(string data, string hexColor)
        {
            timerText.text = $"<color={hexColor}>{data}</color>";
        }

        public void ActiveWarningPanel(float warningPanelMiddleScreenDuration)
        {
            StartCoroutine(WarningScreenMiddle(warningPanelMiddleScreenDuration));
        }

        IEnumerator WarningScreenMiddle(float duration)
        {
            playerWarningText.ActivePanel(true);
            yield return new WaitForSeconds (duration);
            playerWarningText.ActivePanel(false);
            warningGoInWorld.SetActive(true);
        }

        public void ShowRemainingTimeWarning(string remainingTime)
        {
            playerWarningText.SetWarningPanelTextTimer(remainingTime + "s");
        }
    }

}