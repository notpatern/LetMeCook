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
        public GameRecipe gameRecipe;

        public void Init(GameRecipe recipe, Transform parent)
        {
            transform.SetParent(parent);
            gameRecipe = recipe;

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
            Color timeColor = Color.HSVToRGB(120 * timeVisual.fillAmount / 360, .75f, 1);
            timeColor.a = .3f;

            timeVisual.color = timeColor;
            background.color = timeColor;
        }
    }
}