using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // CHANGEZ LE NOM DU TAG ICI SI SA MARCHE PAS
        if (other.tag == "Food")
        {
            var recipeManager = RecipesManager.Instance;
            var food = other.GetComponent<MergedFood>();
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
