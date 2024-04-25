using System.Collections.Generic;
using UnityEngine;

namespace RecipeSystem
{
    [CreateAssetMenu(menuName = "RecipeSystem/RecipesDataBase")]
    public class RecipesDataBase : ScriptableObject
    {
        public List<Recipe> dataBase = new List<Recipe>();

        /// <summary>
        /// Test if a recipe exist for a merged food or a food and return it if it exist, return null otherwise.
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        public Recipe TestFood(FoodSystem.FoodType.Food food)
        {
            // Check all recipes
            foreach (var recipe in dataBase)
            {
                // Check if every ingredient in the food(s) match the recipe
                int checkedFood = 0;
                foreach (var foodData in food.GetFoodDatas())
                    if (recipe.ingredients.Contains(foodData))
                        checkedFood++;

                // Return if it match
                if (checkedFood == recipe.ingredients.Count)
                    return recipe;
            }

            // No recipe match the food(s)
            return null;
        }
    }
}