using Manager;
using UnityEngine;

namespace RecipeSystem.Core
{
    public class TutorialRecipesGoal : RecipeGoal
    {
        [SerializeField] TutorialGameManager m_TutorialgoalRecipesGaol;

        protected override void OnFoodOk(int potentialRecipe, FoodSystem.FoodType.Food currentFood)
        {
            base.OnFoodOk(potentialRecipe, currentFood);
            m_TutorialgoalRecipesGaol.TriggerFinishLevelEndCondition();

        }
    }
}