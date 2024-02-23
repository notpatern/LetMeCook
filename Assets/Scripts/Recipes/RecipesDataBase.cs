using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipesDataBase : ScriptableObject
{
    public List<Recipe> dataBase = new List<Recipe>();

    /// <summary>
    /// Test if a recipe exist for a merged food and return it if it exist, return null otherwise.
    /// </summary>
    /// <param name="food"></param>
    /// <returns></returns>
    public Recipe TestFood(MergedFood food)
    {
        foreach (var recipe in dataBase)
        {
            // je le ferrais quand il y aura le systeme de food
        }
        return null;
    }
}
