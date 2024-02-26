using UnityEngine;

public abstract class Food : MonoBehaviour, IInteractable
{
    public GameObject StartInteraction()
    {
        //TODO give food to the player's hand
        return gameObject;
    }

    public abstract string GetContext();
}
