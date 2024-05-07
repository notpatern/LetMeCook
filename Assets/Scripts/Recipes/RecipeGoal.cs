using PostProcessing;
using UnityEngine;

namespace RecipeSystem.Core
{
    public class RecipeGoal : MonoBehaviour
    {
        [SerializeField] LayerMask foodlayer;
        [SerializeField] RecipesManager recipesManager;
        [SerializeField] Animator mouthAnimator;
        [SerializeField] GameObject receiveParticleParticles;
        [SerializeField] PostProcessingManager postProcessingManager;
        private void OnTriggerEnter(Collider other)
        {
            if (foodlayer == (foodlayer | (1 << other.gameObject.layer)))
            {
                // Try to get the food component
                var food = other.GetComponent<FoodSystem.FoodType.Food>();
                if (!food) return;

                int potentialRecipe = recipesManager.GetRecipeFoodId(food);

                // Completed recipe
                if (potentialRecipe >= 0)
                {
                    OnFoodOk(potentialRecipe);
                }

                Instantiate(receiveParticleParticles, other.bounds.ClosestPoint(other.transform.position), transform.rotation);

                mouthAnimator.SetTrigger("EatFood");
                Destroy(other.gameObject);
            }
        }

        protected virtual void OnFoodOk(int potentialRecipe)
        {
            postProcessingManager.ChangeChromaticAberration(1f, 2.5f);
            recipesManager.CompleteRecipe(potentialRecipe);
        }
    }
}