using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreWorldInfoUI : MonoBehaviour
{
    [SerializeField] TMP_Text textInfo;
    [SerializeField] Color positiveColor;
    [SerializeField] Color negativeColor;
    public void InitText(int amount)
    {
        string preText = "";

        if (amount > 0) preText = $"<color=#{positiveColor.ToHexString()}>";
        else if(amount < 0) preText = $"<color=#{negativeColor.ToHexString()}>";

        textInfo.text = preText + amount.ToString("+#;-#") + "</color>";
    }

    void DestroyGo()
    {
        Destroy(gameObject);
    }
}
