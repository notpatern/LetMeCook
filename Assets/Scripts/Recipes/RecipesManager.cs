using RecipeSystem.Core;
using System.Collections.Generic;
using UnityEngine;

namespace RecipeSystem
{
    public class RecipesManager : MonoBehaviour
    {
        public static RecipesManager Instance { get; private set; }
        public RecipesDataBase dataBase;
        List<Core.GameRecipe> activeRecipes;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            // Debug add random recipe
            if (Input.GetKeyDown(KeyCode.F1))
                AddNewRecipe(dataBase.dataBase[Random.Range(0, dataBase.dataBase.Count - 1)]);

            // Debug complete first recipe in list
            if (Input.GetKeyDown(KeyCode.F2))
                CompleteRecipe(activeRecipes[0].recipe);
        }

        /// <summary>
        /// Append a new recipe to the game.
        /// Return the instantiated GameRecipe.
        /// </summary>
        /// <param name="recipe"></param>
        public GameRecipe AddNewRecipe(Recipe recipe)
        {
            GameRecipe newGameRecipe = new GameRecipe(recipe);
            Instantiate(newGameRecipe);
            activeRecipes.Add(newGameRecipe);
            RecipeUI.Instance.AddNewCard(newGameRecipe);
            return newGameRecipe;
        }

        /// <summary>
        /// Remove a recipe from the game without completing or failing it.
        /// Return true if the recipe was removed successfully.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public bool RemoveRecipe(Recipe recipe)
        {
            GameRecipe gameRecipe = FindRecipe(recipe);
            if (!gameRecipe) return false;

            activeRecipes.Remove(gameRecipe);
            RecipeUI.Instance.RemoveCard(gameRecipe);
            Destroy(gameRecipe.gameObject);
            return true;
        }

        /// <summary>
        /// Complete a recipe from the game and remove it.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public void CompleteRecipe(Recipe recipe)
        {
            GameRecipe gameRecipe = FindRecipe(recipe);
            RecipeUI.Instance.RemoveCard(gameRecipe);
            gameRecipe.CompleteRecipe();
        }

        /// <summary>
        /// Fail a recipe from the game and remove it.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        public void FailRecipe(Recipe recipe)
        {
            GameRecipe gameRecipe = FindRecipe(recipe);
            RecipeUI.Instance.RemoveCard(gameRecipe);
            gameRecipe.FailRecipe();
        }

        GameRecipe FindRecipe(Recipe recipe)
        {
            foreach (GameRecipe gameRecipe in activeRecipes)
                if (gameRecipe.recipe == recipe)
                    return gameRecipe;

            return null;
        }
    }
}
