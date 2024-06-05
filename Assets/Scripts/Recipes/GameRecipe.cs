using UnityEngine;

namespace RecipeSystem.Core
{
    public class GameRecipe
    {
        public Recipe recipe;
        public bool isBonusRecipe;
        public bool isLastRecipe { get; private set; }

        public float timeRemaining;
        public bool isFailed = false;

        public void Init(Recipe recipe, bool isBonusRecipe, bool isLastRecipe)
        {
            this.recipe = recipe;
            this.isBonusRecipe = isBonusRecipe;
            timeRemaining = recipe.secondsToComplete;

            if (isLastRecipe)
            {
                SetAsLastRecipe();
            }
        }

        public void Update()
        {
            if (isFailed) return;

            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                FailRecipe();
            }
        }

        public void SetAsLastRecipe()
        {
            isLastRecipe = true;
        }

        public void FailRecipe()
        {
            isFailed = true;
        }
    }
}