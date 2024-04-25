using UnityEngine;

namespace RecipeSystem.Core
{
    public class RecipeGoal : MonoBehaviour
    {
        LayerMask foodlayer;
        RecipesManager recipesManager;

        private void Start()
        {
            recipesManager = RecipesManager.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == foodlayer)
            {
                // Try to get the food component
                var food = other.GetComponent<FoodSystem.FoodType.Food>();
                var potentialRecipe = recipesManager.dataBase.TestFood(food);

                // Completed recipe
                if (potentialRecipe)
                {
                    recipesManager.CompleteRecipe(potentialRecipe);
                }
            }
        }
    }
}