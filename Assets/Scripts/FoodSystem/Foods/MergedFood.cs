using System.Collections.Generic;

public class MergedFood : Food
{ 
    public List<FoodData> data = new List<FoodData>();
    
    public override string GetContext() => "food bag";
}
