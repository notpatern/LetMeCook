using UnityEngine;

namespace RecipeSystem.Core
{
    public class GameRecipe
    {
        public Recipe recipe;
        public bool isBonusRecipe;

        public float timeRemaining;
        public bool isFailed = false;

        public void Init(Recipe recipe, bool isBonusRecipe)
        {
            this.recipe = recipe;
            this.isBonusRecipe = isBonusRecipe;
            timeRemaining = recipe.secondsToComplete;
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

        public void FailRecipe()
        {
            isFailed = true;
        }
    }
}