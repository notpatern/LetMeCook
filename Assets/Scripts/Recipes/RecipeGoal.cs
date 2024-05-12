using Audio;
using PostProcessing;
using UnityEngine;
using System.Collections.Generic;
using FoodSystem.FoodType;

namespace RecipeSystem.Core
{
    public class RecipeGoal : MonoBehaviour
    {
        [SerializeField] LayerMask foodlayer;
        [SerializeField] RecipesManager recipesManager;
        [SerializeField] Animator mouthAnimator;
        [SerializeField] GameObject receiveParticleParticles;
        [SerializeField] PostProcessingManager postProcessingManager;

        List<Food> processedFood = new List<Food>();

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnter(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (foodlayer == (foodlayer | (1 << other.gameObject.layer)))
            {
                // Try to get the food component
                Food currentFood = other.GetComponent<Food>();

                if (!currentFood || processedFood.Contains(currentFood)) return;

                processedFood.Add(currentFood);
                int currentId = processedFood.Count - 1;

                int potentialRecipe = recipesManager.GetRecipeFoodId(processedFood[currentId]);

                // Completed recipe
                if (potentialRecipe >= 0)
                {
                    OnFoodOk(potentialRecipe);
                }

                Instantiate(receiveParticleParticles, other.bounds.ClosestPoint(other.transform.position), transform.rotation);

                mouthAnimator.SetTrigger("EatFood");
                processedFood.Remove(currentFood);
                Destroy(other.gameObject);
            }
        }

        protected virtual void OnFoodOk(int potentialRecipe)
        {
            AudioManager.s_Instance.PlayOneShot(AudioManager.s_Instance.m_AudioSoundData.m_DeliverySound, transform.position);
            postProcessingManager.ChangeChromaticAberration(1f, 2.5f);
            recipesManager.CompleteRecipe(potentialRecipe);
        }
    }
}