using UnityEngine;
using System.Collections.Generic;
using RecipeSystem;

namespace FoodSystem.FoodType
{
    public class MergedFood : Food
    { 
        [SerializeField] GameObject existingRecipeDecalProjector;
        [SerializeField] GameObject existingFoodFog;

        List<FoodData> data = new List<FoodData>();
        [SerializeField] GameObject infosCanvas;
        [SerializeField] Transform rawCanvasContent;
        [SerializeField] Transform cookedCanvasContent;
        [SerializeField] GameObject contentItemPrefab;

        [SerializeField] GameEventScriptableObject onUpdateRecipeListGiveRecipeManager;

        [SerializeField] MeshRenderer crystalMeshRenderer;
        [SerializeField] Material existingRecipe;
        [SerializeField] Material nonExistingRecipe;

        List<MergedFoodUIItem> mergedFoodUIItems = new List<MergedFoodUIItem>();

        RecipesManager recipesManager;

        protected override void Awake()
        {
            base.Awake();
            onUpdateRecipeListGiveRecipeManager.BindEventAction(OnUpdateRecipeListGiveRecipeManager);
        }

        public void Init(RecipesManager recipesManager)
        {
            this.recipesManager = recipesManager;
        }

        void OnUpdateRecipeListGiveRecipeManager(object arg)
        {
            bool existing = recipesManager.GetRecipeFoodId(this) > -1;
            crystalMeshRenderer.material = existing ? existingRecipe : nonExistingRecipe;
            currentDecalProjector = existing ? existingRecipeDecalProjector : decalProjector;
            currentFoodFog = existing ? existingFoodFog : foodFog;
        }

        public override string GetContext() => "food bag";

        public override void AddFood(SimpleFood newFood)
        {
            data.Add(newFood.data);
            AddFoodInUIContent(newFood.data);

            OnUpdateRecipeListGiveRecipeManager(null);
        }

        public override void AddFood(MergedFood mergedFood)
        {
            foreach(FoodData foodData in mergedFood.GetFoodDatas())
            {
                data.Add(foodData);

                AddFoodInUIContent(foodData);
            }

            OnUpdateRecipeListGiveRecipeManager(null);
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
