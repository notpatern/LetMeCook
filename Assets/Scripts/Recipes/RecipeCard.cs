using UnityEngine;
using TMPro;

namespace RecipeSystem.Core
{
    public class RecipeCard : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] GameObject ingredientsList;
        [SerializeField] GameObject ingredientPrefab;
        GameRecipe recipe;
    }
}