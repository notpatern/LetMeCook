using UnityEngine;

namespace RecipeSystem.Core
{
    public class RecipeGoal : MonoBehaviour
    {
        [SerializeField] LayerMask foodlayer;
        [SerializeField] RecipesManager recipesManager;

        private void OnTriggerEnter(Collider other)
        {
            if (foodlayer == (foodlayer | (1 << other.gameObject.layer)))
            {
                // Try to get the food component
                var food = other.GetComponent<FoodSystem.FoodType.Food>();
                if (!food) return;

                var potentialRecipe = recipesManager.GetRecipeFoodId(food);

                // Completed recipe
                if (potentialRecipe >= 0)
                {
                    Debug.Log(potentialRecipe);
                    recipesManager.CompleteRecipe(potentialRecipe);
                }
                
                Destroy(other.gameObject);
            }
        }
    }
}