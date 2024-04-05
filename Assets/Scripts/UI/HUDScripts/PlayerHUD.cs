using Player.Interaction;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public PlayerInteractionUI playerInteractionUI;
    public Image staminaFill;

    public void UpdateStaminaFill(float percentageAmount)
    {
        Debug.Log("rt : " + percentageAmount);
        staminaFill.fillAmount = percentageAmount;
    }
}
