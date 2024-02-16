using System.Collections.Generic;
using UnityEngine;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] GameObject UIList;
    public static RecipeUI Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void AddNewCard(GameRecipe recipe)
    {
        recipe.transform.parent = UIList.transform;
    }

    public void RemoveNewCard(GameRecipe recipe)
    {
        
    }
}
