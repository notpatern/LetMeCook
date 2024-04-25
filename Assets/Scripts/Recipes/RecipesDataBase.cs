using System;
using UnityEngine;

namespace RecipeSystem
{
    [CreateAssetMenu(menuName = "RecipeSystem/RecipesDataBase")]
    public class RecipesDataBase : ScriptableObject
    {
        public RecipeContainer[] recipesContainers;
        public float m_StartPeriodOffset = 5f;

        [Serializable]
        public class RecipeContainer
        {
            public Recipe m_Recipe;
            public float m_WaitPeriod;
        }


        /// <summary>
        /// Test if a recipe exist for a merged food or a food and return it if it exist, return null otherwise.
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        public Recipe TestFood(FoodSystem.FoodType.Food food)
        {
            // Check all recipes
            foreach (var recipeContainer in recipesContainers)
            {
                // Check if every ingredient in the food(s) match the recipeContainer
                int checkedFood = 0;
                foreach (var foodData in food.GetFoodDatas())
                    if (recipeContainer.m_Recipe.ingredients.Contains(foodData))
                        checkedFood++;

                // Return if it match
                if (checkedFood == recipeContainer.m_Recipe.ingredients.Count)
                    return recipeContainer.m_Recipe;
            }

            // No recipe match the food(s)
            return null;
        }
    }
}