using Audio;
using PostProcessing;
using UnityEngine;
using System.Collections.Generic;
using FoodSystem.FoodType;
using PlayerSystems.MovementFSMCore.DataClass;

namespace RecipeSystem.Core
{
    public class RecipeGoal : MonoBehaviour
    {
        [SerializeField] LayerMask foodlayer;
        [SerializeField] RecipesManager recipesManager;
        [SerializeField] Animator mouthAnimator;
        [SerializeField] GameObject receiveParticleParticles;
        [SerializeField] PostProcessingManager postProcessingManager;

        [SerializeField] CameraData cameraData;
        [SerializeField] GameEventScriptableObject onShake;

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

                int potentialRecipe = -1;

                if (currentFood)
                {
                    potentialRecipe = recipesManager.GetRecipeFoodId(currentFood);
                }

                // Completed recipe
                if (potentialRecipe >= 0)
                {
                    OnFoodOk(potentialRecipe, currentFood);
                }
                else {
                    float[] shakeValues = {
                        cameraData.rejectedShakeDuration, cameraData.rejectedShakeStrength, cameraData.rejectedShakeVibrator, cameraData.rejectedShakeRandomness
                    };
                    onShake.TriggerEvent(shakeValues);
                }

                processedFood.Add(currentFood);

                Instantiate(receiveParticleParticles, other.bounds.ClosestPoint(other.transform.position), transform.rotation);

                mouthAnimator.SetTrigger("EatFood");
                Destroy(other.gameObject);
            }
        }

        protected virtual void OnFoodOk(int potentialRecipe, Food currentFood)
        {
            if (!currentFood || processedFood.Contains(currentFood)) {
                return;
            }

            AudioManager.s_Instance.PlayOneShot(AudioManager.s_Instance.m_AudioSoundData.m_DeliverySound, transform.position);
            float[] shakeValues = {
                cameraData.acceptedShakeDuration, cameraData.acceptedShakeStrength, cameraData.acceptedShakeVibrator, cameraData.acceptedShakeRandomness
            };
            onShake.TriggerEvent(shakeValues);
            postProcessingManager.ChangeChromaticAberration(1f, 2.5f);

            recipesManager.CompleteRecipe(potentialRecipe, currentFood.transform.position);

            processedFood.Remove(currentFood);
        }
    }
}