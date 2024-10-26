using System.Linq;
using FoodSystem;
using FoodSystem.FoodMachinery;
using UnityEngine;

namespace RecipeSystem.Core
{
    public class UniqueRecipeGoal : FoodCollector
    {
        [SerializeField] Animator animator;
        [SerializeField] FoodData[] requestedRecipe;

        bool m_completed = false;
        
        protected override void OnFoodCollected()
        {
            if (requestedRecipe == null || !IsRecipeRight(collectedFood.GetFoodDatas().ToArray())) return;
            
            animator.SetTrigger("Completed");
        }

        bool IsRecipeRight(FoodData[] foodToCheck)
        {
            if (foodToCheck.Length != requestedRecipe.Length) return false;

            foreach (FoodData foodData in foodToCheck)
            {
                //TODO test
            }

            return foodToCheck.Where((t, i) => t == requestedRecipe[i]).Any();
        }
    }
}