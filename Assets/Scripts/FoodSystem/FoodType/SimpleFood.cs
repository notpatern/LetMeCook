using UnityEngine.Serialization;
using System.Collections.Generic;

namespace FoodSystem.FoodType
{
    public class SimpleFood : Food
    {
        [FormerlySerializedAs("_data")] public FoodData data;

        public override string GetContext() => data.foodName;

        public override void AddFood(SimpleFood newFood)
        { }

        public override void AddFood(MergedFood mergedFood)
        { }

        public override List<FoodData> GetFoodDatas()
        {
            return new List<FoodData> { data };
        }
    }
}
