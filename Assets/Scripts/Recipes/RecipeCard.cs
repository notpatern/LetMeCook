using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RecipeSystem.Core
{
    public class RecipeCard : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] Image timeVisual;
        [SerializeField] Image background;
        [SerializeField] GameObject ingredientsList;
        [SerializeField] GameObject ingredientPrefab;
        GameRecipe gameRecipe;

        void Start()
        {
            titleText.text = gameRecipe.recipe.nametag;
            foreach (var ingredient in gameRecipe.recipe.ingredients)
            {
                var newIngredient = Instantiate(ingredientPrefab, ingredientsList.transform);
                newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.foodName;
            }
        }

        void Update()
        {
            timeVisual.fillAmount = gameRecipe.timeRemaining / gameRecipe.recipe.secondsToComplete;
            Color timeColor = Color.HSVToRGB((120 - (120 * timeVisual.fillAmount)) / 360, 1, 1);
            timeVisual.color = timeColor;
            background.color = timeColor;
        }
    }
}