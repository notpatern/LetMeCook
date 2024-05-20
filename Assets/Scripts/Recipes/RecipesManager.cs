using Manager;
using RecipeSystem.Core;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using FoodSystem.FoodType;
using FoodSystem;
using Audio;

namespace RecipeSystem
{
    public class RecipesManager : MonoBehaviour
    {
        public RecipesDataBase dataBase;
        GameManager gameManager;
        List<GameRecipe> activeRecipes = new List<GameRecipe>();
        [SerializeField] Transform SpawnVoice3DPosition;
        RecipeUI recipeUI;

        protected int recipesRemoved = 0;
        protected int mainRecipeStarted = 0;

        [SerializeField] AudioQueueComponent m_AudioQueueComponent;

        [SerializeField] GameEventScriptableObject m_OnUpdateRecipeListGiveRecipeManager;

        public void Init(GameManager gameManager, RecipeUI recipeUI)
        {
            this.gameManager = gameManager;
            this.recipeUI = recipeUI;

            StartRecipesList();
        }

        void StartRecipesList()
        {
            StartCoroutine(StartRecipesContainerPeriod());
        }

        private void Update()
        {
            for(int i=0; i<activeRecipes.Count; i++)
            {
                activeRecipes[i].Update();

                if (activeRecipes[i].isFailed)
                {
                    RemoveRecipe(i);
                    return;
                }
            }
        }

        IEnumerator StartRecipesContainerPeriod()
        {
            yield return new WaitForSeconds(dataBase.m_StartPeriodOffset);

            for (int i = 0; i < dataBase.recipesContainers.Length; i++)
            {
                mainRecipeStarted++;
                AddNewRecipe(dataBase.recipesContainers[i].m_Recipe, false);
                yield return new WaitForSeconds(dataBase.recipesContainers[i].m_WaitPeriod);
            }
        }

        /// <summary>
        /// Append a new recipe to the game.
        /// Return the instantiated GameRecipe.
        /// </summary>
        /// <param name="recipe"></param>
        GameRecipe AddNewRecipe(Recipe recipe, bool isBonus)
        {
            GameRecipe newGameRecipe = new GameRecipe();
            newGameRecipe.Init(recipe, isBonus);

            m_AudioQueueComponent.StartSound(recipe.vocaloidVoice);

            activeRecipes.Add(newGameRecipe);
            recipeUI.AddNewCard(newGameRecipe, dataBase.recipesContainers.Length == mainRecipeStarted);

            if (!isBonus)
            {
                gameManager.AddRecipesCount(1);
            }

            m_OnUpdateRecipeListGiveRecipeManager.TriggerEvent(null);

            return newGameRecipe;
        }

        /// <summary>
        /// Remove a recipe from the game without completing or failing it.
        /// Return true if the recipe was removed successfully.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        bool RemoveRecipe(int recipeId)
        {
            GameRecipe gameRecipe = activeRecipes[recipeId];

            recipeUI.RemoveCard(gameRecipe);
            activeRecipes.RemoveAt(recipeId);
            recipesRemoved++;

            if (dataBase.recipesContainers.Length - recipesRemoved == 0)
            {
                gameManager.ForceEndConditionTimerValue(0f);
            }
            else if(activeRecipes.Count == 0 && dataBase.randomFillerRecipes.Length > 0)
            {
                recipesRemoved--;
                AddNewRecipe(dataBase.randomFillerRecipes[Random.Range(0, dataBase.randomFillerRecipes.Length)], true);
            }

            m_OnUpdateRecipeListGiveRecipeManager.TriggerEvent(null);

            return true;
        }

        /// <summary>
        /// Complete a recipe from the game and remove it.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public void CompleteRecipe(int recipeId)
        {
            GameRecipe gameRecipe = activeRecipes[recipeId];
            RemoveRecipe(recipeId);

            gameManager.AddAcomplishedRecipes(gameRecipe);
            gameManager.AddScore(gameRecipe.recipe.addedScore);
        }

        /// <summary>
        /// Test if a recipe exist for a merged food or a food and return it id if it exist, return -1 otherwise.
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        public int GetRecipeFoodId(Food food)
        {
            List<FoodData> currentfoodDatas = food.GetFoodDatas();
            List<FoodData> currentContainerFoodDatas;
            int foodDatasCount = currentfoodDatas.Count;

            // Check all recipes
            for (int i = 0; i < activeRecipes.Count; i++)
            {
                // No need to check if it's isn't the same size
                if (activeRecipes[i].recipe.ingredients.Count != foodDatasCount)
                {
                    continue;
                }

                currentContainerFoodDatas = new List<FoodData>(activeRecipes[i].recipe.ingredients);
                currentfoodDatas = new List<FoodData>(food.GetFoodDatas());

                bool isOk = true;
                // Check if every ingredient in the food(s) match the activeRecipes
                foreach(FoodData foodData in food.GetFoodDatas())
                {
                    // Remove the item every time to handle the exemple : 2 pizza and 3 ravioli in recipe and 3 pizza and 2 ravioli in hand
                    if (currentContainerFoodDatas.Contains(foodData) && currentfoodDatas.Contains(foodData))
                    {
                        currentContainerFoodDatas.Remove(foodData);
                        currentfoodDatas.Remove(foodData);
                    }
                    else
                    {
                        isOk = false;
                        break;
                    }
                }

                if (isOk)
                {
                    return i;
                }
            }

            // No recipe match the food(s)
            return -1;
        }

        public int GetMaxScoreOnMainRecipesFeed()
        {
            int result = 0;

            for (int i = 0; i < dataBase.recipesContainers.Length; i++)
            {
                result += dataBase.recipesContainers[i].m_Recipe.addedScore;
            }

            return result;
        }

        public float GetLevelDurationBasedOnRecipesDataBase()
        {
            float result = dataBase.m_StartPeriodOffset;
            for(int i=0; i < dataBase.recipesContainers.Length - 1; i++)
            {
                result += dataBase.recipesContainers[i].m_WaitPeriod;
            }

            result += dataBase.recipesContainers[dataBase.recipesContainers.Length - 1].m_Recipe.secondsToComplete;

            return result;
        }
    }
}
