using System.Collections.Generic;
using UnityEngine;

public class RecipesManager : MonoBehaviour
{
    public static RecipesManager Instance { get; private set; }
    public RecipesDataBase dataBase;
    List<GameRecipe> activeRecipes;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Append a new recipe to the game.
    /// Return the instantiated GameRecipe.
    /// </summary>
    /// <param name="recipe"></param>
    public GameRecipe AddNewRecipe(Recipe recipe)
    {
        var newGameRecipe = new GameRecipe(recipe);
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
        var gameRecipe = FindRecipe(recipe);
        if (!gameRecipe) return false;

        activeRecipes.Remove(gameRecipe);
        Destroy(gameRecipe.gameObject);
        return true;
    }

    public void CompleteRecipe(Recipe recipe)
    {
        var gameRecipe = FindRecipe(recipe);
        RecipeUI.Instance.RemoveNewCard(gameRecipe);
        gameRecipe.CompleteRecipe();
    }

    public void FailRecipe(Recipe recipe)
    {
        var gameRecipe = FindRecipe(recipe);
        RecipeUI.Instance.RemoveNewCard(gameRecipe);
        gameRecipe.FailRecipe();
    }

    GameRecipe FindRecipe(Recipe recipe)
    {
        foreach (var gameRecipe in activeRecipes)
            if (gameRecipe.recipe == recipe)
                return gameRecipe;

        return null;
    }
}
