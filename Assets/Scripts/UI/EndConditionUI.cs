 using UnityEngine;
using TMPro;
using System.Collections;

namespace UI
{
    public class EndConditionUI : MonoBehaviour
    {
        [SerializeField] TMP_Text timerText;
        [SerializeField] TMP_Text timeRemainingWarning;
        [SerializeField] GameObject warningGo;
        [SerializeField] GameObject warningGoInHand;

        public void Init(Transform timerParent)
        {
            timerText.transform.SetParent(timerParent);
            timerText.transform.SetAsFirstSibling();
            warningGoInHand.transform.SetParent(timerParent);
            timerText.transform.SetAsFirstSibling();

            timerText.text = "";
            timeRemainingWarning.text = "";
            warningGo.SetActive(false);
            warningGoInHand.SetActive(false);
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
            warningGo.SetActive(true);
            yield return new WaitForSeconds (duration);
            warningGo.SetActive(false);
            warningGoInHand.SetActive(true);
        }

        public void ShowRemainingTimeWarning(string remainingTime)
        {
            timeRemainingWarning.text = remainingTime + "s";
        }
    }

}