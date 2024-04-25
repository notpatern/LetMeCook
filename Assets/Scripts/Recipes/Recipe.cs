using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace RecipeSystem
{
    [CreateAssetMenu(menuName = "RecipeSystem/Recipe")]
    public class Recipe : ScriptableObject
    {
        public EventReference vocaloidVoice;
        public string nametag = "Undefined";
        public float secondsToComplete = 10.0f;
        public List<FoodSystem.FoodData> ingredients = new List<FoodSystem.FoodData>();
        public int addedScore = 150;
    }
}

