using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDropper : MonoBehaviour, IInteractable
{
    [SerializeField] FoodData m_givingFood;
    
    public GameObject StartInteraction()
    {
        //TODO give food to the player's hand
        return m_givingFood.foodPrefab;
    }

    public string GetContext() => m_givingFood.foodName;
}
