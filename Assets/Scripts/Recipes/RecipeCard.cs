using UnityEngine;
using TMPro;

public class RecipeCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] GameObject ingredientsList;
    [SerializeField] GameObject ingredientPrefab;
    GameRecipe recipe;
}
