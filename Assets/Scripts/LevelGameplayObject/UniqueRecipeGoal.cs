using System;
using System.Collections.Generic;
using FoodSystem;
using FoodSystem.FoodMachinery;
using UnityEngine;
using UnityEngine.Events;

namespace LevelGameplayObject
{
    public class UniqueRecipeGoal : FoodCollector
    {
        [SerializeField] Animator animator;
        [SerializeField] FoodData[] requestedRecipe;

        public UnityEvent<UniqueRecipeGoal> onRecipeComplete;

        void Awake()
        {
            onRecipeComplete ??= new UnityEvent<UniqueRecipeGoal>();
        }

        protected override void OnFoodCollected()
        {
            if (requestedRecipe == null || !IsRecipeRight(collectedFood.GetFoodDatas().ToArray())) return;

            onRecipeComplete.Invoke(this);
            animator.SetTrigger("Completed");
        }

        bool IsRecipeRight(FoodData[] foodToCheck)
        {
            if (foodToCheck.Length != requestedRecipe.Length) return false;

            List<FoodData> tempFoodToCheck = new List<FoodData>(foodToCheck);
            List<FoodData> tempRequestRecipe = new List<FoodData>(requestedRecipe);

            foreach (FoodData foodData in foodToCheck)
            {
                // From RecipesManager.cs
                // Remove the item every time to handle the example : 2 pizza and 3 ravioli in recipe and 3 pizza and 2 ravioli in hand
                if (tempFoodToCheck.Contains(foodData) && tempRequestRecipe.Contains(foodData))
                {
                    tempFoodToCheck.Remove(foodData);
                    tempRequestRecipe.Remove(foodData);
                }
                else
                    return false;
            }

            return true;
        }
    }
}