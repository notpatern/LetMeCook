using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreWorldInfoUI : MonoBehaviour
{
    [SerializeField] TMP_Text textInfo;
    [SerializeField] TMP_Text timeText;
    [SerializeField] Color positiveColor;
    [SerializeField] Color timeColor;
    [SerializeField] Color negativeColor;
    public void InitText(int amount, int timeAmount = 0)
    {
        string preText = "";

        if (timeAmount > 0) {
            if (timeAmount > 0) preText = $"<color=#{timeColor.ToHexString()}>";
            timeText.text = preText + timeAmount.ToString("+#;-#") + "</color>";
        }
        else {
            timeText.text = "";
        }

        if (amount > 0) preText = $"<color=#{positiveColor.ToHexString()}>";
        else if(amount < 0) preText = $"<color=#{negativeColor.ToHexString()}>";

        textInfo.text = preText + amount.ToString("+#;-#") + "</color>";
    }

    void DestroyGo()
    {
        Destroy(gameObject);
    }
}
