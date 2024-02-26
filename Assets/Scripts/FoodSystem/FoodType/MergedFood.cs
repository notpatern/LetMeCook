using System.Collections.Generic;

namespace FoodSystem.FoodType
{
    public class MergedFood : Food
    { 
        public List<FoodData> data = new List<FoodData>();
    
        public override string GetContext() => "food bag";
    }
}
