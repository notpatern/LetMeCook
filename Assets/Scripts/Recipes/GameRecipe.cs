using UnityEngine;

namespace RecipeSystem.Core
{
    public class GameRecipe
    {
        public Recipe recipe;

        [HideInInspector] public float timeRemaining;
        [HideInInspector] public bool isFailed = false;

        public void Init(Recipe recipe)
        {
            this.recipe = recipe;
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
            Debug.Log("fail");
        }
    }
}