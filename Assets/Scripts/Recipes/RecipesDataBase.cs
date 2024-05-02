using System;
using UnityEngine;

namespace RecipeSystem
{
    [CreateAssetMenu(menuName = "RecipeSystem/RecipesDataBase")]
    public class RecipesDataBase : ScriptableObject
    {
        public RecipeContainer[] recipesContainers;
        [Tooltip("Must have same points")]
        public RecipeContainer[] randomFillerRecipes;
        public float m_StartPeriodOffset = 5f;

        [Serializable]
        public class RecipeContainer
        {
            public Recipe m_Recipe;
            public float m_WaitPeriod;
        }
    }
}