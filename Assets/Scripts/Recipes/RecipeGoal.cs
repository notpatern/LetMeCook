using UnityEngine;

namespace RecipeSystem.Core
{
    public class RecipeGoal : MonoBehaviour
    {
        [SerializeField] LayerMask foodlayer;
        RecipesManager recipesManager;

        private void Start()
        {
            recipesManager = RecipesManager.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (foodlayer == (foodlayer | (1 << other.gameObject.layer)))
            {
                // Try to get the food component
                var food = other.GetComponent<FoodSystem.FoodType.Food>();
                var potentialRecipe = recipesManager.dataBase.TestFood(food);

                // Completed recipe
                if (potentialRecipe)
                {
                    recipesManager.CompleteRecipe(potentialRecipe);
                }
                else
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}