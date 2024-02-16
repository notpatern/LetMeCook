using UnityEngine.Serialization;

public class SimpleFood : Food
{
    [FormerlySerializedAs("_data")] public FoodData data;

    public override string GetContext() => data.foodName;
}
