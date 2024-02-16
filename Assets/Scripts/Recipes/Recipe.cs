using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : ScriptableObject
{
    public string nametag = "Undefined";
    public float secondsToComplete = 10.0f;
    public List<Food> ingredients = new List<Food>();
}
