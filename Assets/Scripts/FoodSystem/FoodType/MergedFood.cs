using UnityEngine;
using System.Collections.Generic;

namespace FoodSystem.FoodType
{
    public class MergedFood : Food
    { 
        List<FoodData> data = new List<FoodData>();
        [SerializeField] GameObject infosCanvas;
        [SerializeField] Transform rawCanvasContent;
        [SerializeField] Transform cookedCanvasContent;
        [SerializeField] GameObject contentItemPrefab;

        List<MergedFoodUIItem> mergedFoodUIItems = new List<MergedFoodUIItem>();

        public override string GetContext() => "food bag";

        public override void AddFood(SimpleFood newFood)
        {
            data.Add(newFood.data);
            AddFoodInUIContent(newFood.data);
        }

        public override void AddFood(MergedFood mergedFood)
        {
            foreach(FoodData foodData in mergedFood.GetFoodDatas())
            {
                data.Add(foodData);

                AddFoodInUIContent(foodData);
            }
        }

        void AddFoodInUIContent(FoodData foodData)
        {
            MergedFoodUIItem uiItem = GetMergedUIItem(foodData);

            if(!uiItem)
            {
                Transform content = foodData.HasNextTransformatedState() ? rawCanvasContent : cookedCanvasContent;

                MergedFoodUIItem newUIItem = Instantiate(contentItemPrefab, content).GetComponent<MergedFoodUIItem>();
                newUIItem.InitItem("x", foodData);
                mergedFoodUIItems.Add(newUIItem);
            }
            else
            {
                uiItem.IncrementFoodCounter();
            }
        }

        public override List<FoodData> GetFoodDatas()
        {
            return data;
        }

        public override void PutInHand(Transform hand)
        {
            base.PutInHand(hand);

            infosCanvas.transform.localRotation = Quaternion.Euler(infosCanvas.transform.localRotation.eulerAngles.x, hand.localRotation.eulerAngles.y + 180, infosCanvas.transform.localRotation.eulerAngles.z);

            infosCanvas.SetActive(true);
        }

        public override void RemoveFromHand()
        {
            base.RemoveFromHand();
            infosCanvas.SetActive(false);
        }

        MergedFoodUIItem GetMergedUIItem(FoodData foodData)
        {
            foreach(MergedFoodUIItem mergedFoodUIItem in mergedFoodUIItems)
            {
                if(mergedFoodUIItem.m_FoodData == foodData)
                {
                    return mergedFoodUIItem;
                }
            }

            return null;
        }
    }
}
