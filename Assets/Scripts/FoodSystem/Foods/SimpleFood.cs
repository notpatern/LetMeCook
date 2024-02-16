public class SimpleFood : Food
{
    FoodData _data;

    public override string GetContext() => _data.foodName;
}
