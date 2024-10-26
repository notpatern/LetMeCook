using System.Collections.Generic;
using UnityEngine;

namespace LevelGameplayObject
{
    public class UniqueRecipeGoalContainer : MonoBehaviour
    {
        [SerializeField] List<UniqueRecipeGoal> recipeGoals = new List<UniqueRecipeGoal>();
        [SerializeField] Animator _objectToAnimate;
        
        static readonly int Show = Animator.StringToHash("Show");

        void Start()
        {
            foreach (UniqueRecipeGoal recipeGoal in recipeGoals)
            {
                recipeGoal.onRecipeComplete.AddListener(RemoveRecipeGoal);
            }
            
            
            //TODO REMOVE THIS
            //_objectToAnimate.SetTrigger(Show);
            
            
            
        }

        void RemoveRecipeGoal(UniqueRecipeGoal recipeGoal)
        {
            recipeGoals.Remove(recipeGoal);
            
            if (recipeGoals.Count == 0)
            {
                _objectToAnimate.SetTrigger(Show);
            }
        }
    }
}