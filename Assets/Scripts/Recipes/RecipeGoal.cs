using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RecipeSystem.Core
{
    public class RecipeGoal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 8)
            {
                // Try to get the food component
                var recipeManager = RecipesManager.Instance;
                var food = other.GetComponent<FoodSystem.FoodType.Food>();
                var potentialRecipe = recipeManager.dataBase.TestFood(food);

                // Completed recipe
                if (potentialRecipe)
                    recipeManager.CompleteRecipe(potentialRecipe);

                // Recipe dosn't exist
                else
                    print("bruh");

            }
        }
    }
}