using Manager;
using UnityEngine;

namespace RecipeSystem.Core
{
    public class TutorialUniqueRecipeGoal : RecipeGoal
    {
        protected override void OnFoodOk(int potentialRecipe, FoodSystem.FoodType.Food currentFood)
        {
            base.OnFoodOk(potentialRecipe, currentFood);
            //m_TutorialgoalRecipesGaol.TriggerFinishLevelEndCondition();
            Debug.Log("MIAM MIAM");

        }
    }
}