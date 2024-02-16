using System.Collections.Generic;
using UnityEngine;

public class MergedFood : Food
{ 
    List<FoodData> _data = new List<FoodData>();
    
    public override string GetContext() => "food bag";
}
