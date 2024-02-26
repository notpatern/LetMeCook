using UnityEngine.Serialization;

namespace FoodSystem.FoodType
{
    public class SimpleFood : FoodType.Food
    {
        [FormerlySerializedAs("_data")] public FoodData data;

        public override string GetContext() => data.foodName;
    }
}
