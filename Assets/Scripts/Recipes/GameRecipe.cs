using UnityEngine;

namespace RecipeSystem.Core
{
    public class GameRecipe : MonoBehaviour
    {
        public Recipe recipe;

        [HideInInspector] public float timeRemaining;
        bool failed;
        bool completed;

        public void Init(Recipe recipe)
        {
            this.recipe = recipe;
            timeRemaining = recipe.secondsToComplete;
        }

        void Update()
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0 && !failed)
                FailRecipe();
        }

        public void FailRecipe()
        {
            if (failed) return;
            failed = true;
            RecipesManager.Instance.RemoveRecipe(recipe);
        }

        public void CompleteRecipe()
        {
            if (completed) return;
            completed = true;
            // ---- Ajouter des points ici
            RecipesManager.Instance.RemoveRecipe(recipe);
        }
    }
}