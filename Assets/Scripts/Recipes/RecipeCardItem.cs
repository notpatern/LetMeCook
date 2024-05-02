using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCardItem : MonoBehaviour
{
    [SerializeField] Image m_ElementImage;

    public void InitItem(Sprite icon)
    {
        m_ElementImage.sprite = icon;
    }
}
