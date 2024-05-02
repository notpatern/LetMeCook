using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FoodSystem;

public class MergedFoodUIItem : MonoBehaviour
{
    [SerializeField] TMP_Text m_TitleText;
    [SerializeField] Image m_ContentIcon;
    public FoodData m_FoodData { private set; get; }

    string m_Prefix;
    int foodCounter = 1;

    public void InitItem(string prefix, FoodData foodData)
    {
        foodCounter = 1;
        m_Prefix = prefix;
        m_FoodData = foodData;
        m_ContentIcon.sprite = foodData.icon;
        UpdateText();
    }

    public void IncrementFoodCounter()
    {
        foodCounter++;
        UpdateText();
    }

    void UpdateText()
    {
        m_TitleText.text = m_Prefix + foodCounter;
    }
}
