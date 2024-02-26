using UnityEngine;

public class FoodDropper : MonoBehaviour, IInteractable
{
    [SerializeField] SimpleFood givingFood;
    
    public GameObject StartInteraction()
    {
        //TODO give food to the player's hand
        return givingFood.data.foodPrefab;
    }

    public string GetContext() => "dropper (" + givingFood.GetContext() + ")";
}
