using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FoodSystem;

namespace RecipeSystem.Core
{
    public class RecipeCard : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] Image timeVisual;
        [SerializeField] Image background;
        [SerializeField] Transform ingredientsListContent;
        [SerializeField] GameObject ingredientPrefab;
        [SerializeField] GameObject rectList;
        [SerializeField] GameObject lastRecipeIndicator;
        [SerializeField] GameObject bonusRecipeIndicator;
        [SerializeField] GameObject WarningRecipeIndicator;
        [SerializeField] TMP_Text scoreGive;
        [SerializeField] TMP_Text recipeTime;
        public GameRecipe gameRecipe;
        float timerRemainingBefore = 10f;
        bool isInWarningState = false;

        public void Init(GameRecipe recipe, Transform parent, bool isLastRecipe)
        {
            transform.SetParent(parent);
            gameRecipe = recipe;

            titleText.text = gameRecipe.recipe.nametag;

            if (isLastRecipe)
            {
                SetUIAsLastRecipe();
            }
            else
            {
                lastRecipeIndicator.SetActive(false);
            }

            bonusRecipeIndicator.SetActive(gameRecipe.isBonusRecipe);

            FoodData lastIngredient = null;
            GameObject currentParentContent = null;
            foreach (FoodData ingredient in gameRecipe.recipe.ingredients)
            {
                if (lastIngredient != ingredient)
                {
                    currentParentContent = Instantiate(rectList, ingredientsListContent);
                }

                Instantiate(ingredientPrefab, currentParentContent.transform).GetComponent<RecipeCardItem>().InitItem(ingredient.icon);

                lastIngredient = ingredient;
            }

            scoreGive.text = "+" + gameRecipe.recipe.addedScore;
            GetComponent<RectTransform>().localScale = Vector3.one;
            WarningRecipeIndicator.SetActive(false);
        }

        public void SetUIAsLastRecipe()
        {
            lastRecipeIndicator.SetActive(true);
        }

        void Update()
        {
            timeVisual.fillAmount = gameRecipe.timeRemaining / gameRecipe.recipe.secondsToComplete;
            Color timeColor = Color.HSVToRGB(120 * timeVisual.fillAmount / 360, .75f, 1);
            timeColor.a = .3f;

            timeVisual.color = timeColor;
            background.color = timeColor;

            recipeTime.text = Mathf.FloorToInt(gameRecipe.timeRemaining) + "s";

            if(!isInWarningState && gameRecipe.timeRemaining < timerRemainingBefore)
            {
                isInWarningState = true;
                WarningRecipeIndicator.SetActive(true);
            }
        }
    }
}