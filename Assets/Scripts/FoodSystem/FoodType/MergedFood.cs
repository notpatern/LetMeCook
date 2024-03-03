using System.Collections.Generic;

namespace FoodSystem.FoodType
{
    public class MergedFood : Food
    { 
        public List<FoodData> data = new List<FoodData>();
    
        public override string GetContext() => "food bag";

        public override void AddFood(SimpleFood newFood)
        {
            data.Add(newFood.data);
        }

        public override void AddFood(MergedFood mergedFood)
        {
            foreach(FoodData foodData in mergedFood.data)
                data.Add(foodData);
        }

        public override List<FoodData> GetFoodDatas()
        {
            return data;
        }
    }
}
