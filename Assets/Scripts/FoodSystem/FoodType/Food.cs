using Player.Interaction;
using UnityEngine;
using System.Collections.Generic;

namespace FoodSystem.FoodType
{
    public abstract class Food : MonoBehaviour, IInteractable
    {
        public GameObject StartInteraction()
        {
            //TODO give food to the player's hand
            return gameObject;
        }

        public abstract string GetContext();

        public abstract void AddFood(SimpleFood newFood);
        public abstract void AddFood(MergedFood mergedFood);
        public abstract List<FoodData> GetFoodDatas();
    }
}
