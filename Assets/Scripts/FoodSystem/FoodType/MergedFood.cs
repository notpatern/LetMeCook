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
            Instantiate(contentItemPrefab, canvasContent).GetComponent<MergedFoodUIItem>().SetText(newFood.data.foodName);
        }

        public override void AddFood(MergedFood mergedFood)
        {
            foreach(FoodData foodData in mergedFood.GetFoodDatas())
            {
                data.Add(foodData);
                Instantiate(contentItemPrefab, canvasContent).GetComponent<MergedFoodUIItem>().SetText(foodData.foodName);
            }
        }

        public override List<FoodData> GetFoodDatas()
        {
            return data;
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
