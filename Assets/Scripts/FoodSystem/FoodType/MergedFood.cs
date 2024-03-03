using UnityEngine;
using System.Collections.Generic;

namespace FoodSystem.FoodType
{
    public class MergedFood : Food
    { 
        List<FoodData> data = new List<FoodData>();
        [SerializeField] GameObject infosCanvas;
        [SerializeField] Transform canvasContent;
        [SerializeField] GameObject contentItemPrefab; 
    
        public override string GetContext() => "food bag";

        public override void AddFood(SimpleFood newFood)
        {
            data.Add(newFood.data);
        }

        public override void AddFood(MergedFood mergedFood)
        {
            foreach(FoodData foodData in mergedFood.data)
            {
                data.Add(foodData);
                //TODO : spawn item in infos parent
            }
        }

        public override List<FoodData> GetFoodDatas()
        {
            return data;
            
                //TODO : spawn item in infos parent
        }

        public override void PutInHand(Transform hand)
        {
            base.PutInHand(hand);
            infosCanvas.SetActive(true);
        }

        public override void RemoveFromHand()
        {
            base.RemoveFromHand();
            infosCanvas.SetActive(false);
        }
    }
}
