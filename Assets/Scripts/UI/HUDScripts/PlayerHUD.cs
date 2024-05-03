using Player.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public PlayerInteractionUI playerInteractionUI;
    public Image staminaFill;

    [SerializeField] TMP_Text scoreUI;

    private void Awake()
    {
        UpdateScoreUI(0);
    }

    public void UpdateStaminaFill(float percentageAmount)
    {
        staminaFill.fillAmount = percentageAmount;
    }

    public void UpdateScoreUI(int scoreAmouunt) 
    {
        scoreUI.text = "Score : " + scoreAmouunt.ToString();
    }
}
