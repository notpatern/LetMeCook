using UnityEngine;
using TMPro;

public class MergedFoodUIItem : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;

    public void SetText(string name)
    {
        titleText.text = name;
    }
}
