 using UnityEngine;
using TMPro;

namespace UI
{
    public class EndConditionUI : MonoBehaviour
    {
        [SerializeField] TMP_Text timerText;

        void Start()
        {
            timerText.text = "";
        }

        public void UpdateText(string data, string hexColor)
        {
            timerText.text = $"<color={hexColor}>{data}</color>";
        }
    }

}