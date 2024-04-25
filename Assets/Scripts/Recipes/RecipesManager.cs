using Manager;
using RecipeSystem.Core;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Audio;

namespace RecipeSystem
{
    public class RecipesManager : MonoBehaviour
    {
        public static RecipesManager Instance { get; private set; }
        public RecipesDataBase dataBase;
        [SerializeField] GameManager gameManager;
        List<GameRecipe> activeRecipes = new List<GameRecipe>();
        [SerializeField] Transform SpawnVoice3DPosition;

        void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            //var randomRecipe = dataBase.dataBase[Random.Range(0, dataBase.dataBase.Count-1)];
            //AddNewRecipe(randomRecipe);
            StartCoroutine(StartRecipesContainerPeriod());
        }

        IEnumerator StartRecipesContainerPeriod()
        {
            yield return new WaitForSeconds(dataBase.m_StartPeriodOffset);

            for (int i = 0; i < dataBase.recipesContainers.Length; i++)
            {
                AddNewRecipe(dataBase.recipesContainers[i].m_Recipe);
                yield return new WaitForSeconds(dataBase.recipesContainers[i].m_WaitPeriod);
            }
        }

        /// <summary>
        /// Append a new recipe to the game.
        /// Return the instantiated GameRecipe.
        /// </summary>
        /// <param name="recipe"></param>
        public GameRecipe AddNewRecipe(Recipe recipe)
        {
            GameObject newGameObject = new GameObject(recipe.nametag);
            GameRecipe newGameRecipe = newGameObject.AddComponent<GameRecipe>();
            newGameRecipe.Init(recipe);

            AudioManager.s_Instance.PlayOneShot(recipe.vocaloidVoice, SpawnVoice3DPosition.position);

            activeRecipes.Add(newGameRecipe);
            RecipeUI.Instance.AddNewCard(newGameRecipe);

            gameManager.AddRecipesCount(1);

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

            gameManager.AddScore(recipe.addedScore);
            gameManager.AddAcomplishedRecipes(1);
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
