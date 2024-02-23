using System.Collections;
using UnityEngine;

public class GameRecipe : MonoBehaviour
{
    public Recipe recipe;

    float timeRemaining;
    bool failed;
    bool completed;

    public GameRecipe(Recipe recipe)
    {
        this.recipe = recipe;
        timeRemaining = recipe.secondsToComplete;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            FailRecipe();
        }
    }

    public void FailRecipe()
    {
        if (failed) return;
        failed = true;
        StartCoroutine("_FailRecipe");
    }
    IEnumerable _FailRecipe()
    {
        // Make funny animation
        yield return new WaitForSeconds(1f);
        RecipesManager.Instance.RemoveRecipe(recipe);
    }

    public void CompleteRecipe()
    {
        if (completed) return;
        completed = true;
        StartCoroutine("_CompleteRecipe");
    }

    IEnumerable _CompleteRecipe()
    {
        // Make funny animation
        yield return new WaitForSeconds(1f);
        RecipesManager.Instance.RemoveRecipe(recipe);
    }
}
