using Manager;
using UnityEngine;

namespace RecipeSystem.Core
{
    public class TutorialRecipesGoal : RecipeGoal
    {
        [SerializeField] TutorialGameManager m_TutorialgoalRecipesGaol;

        protected override void OnFoodOk(int potentialRecipe)
        {
            base.OnFoodOk(potentialRecipe);
            m_TutorialgoalRecipesGaol.TriggerFinishLevelEndCondition();

        }
    }
}