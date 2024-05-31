using Player.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public PlayerInteractionUI playerInteractionUI;
    public Image staminaFill;

    [SerializeField] TMP_Text scoreToReach;
    [SerializeField] GameObject[] nextStars;

    private void Awake()
    {
        for (int i = 0; i < nextStars.Length; i++)
        {
            nextStars[i].SetActive(false);
        }
    }

    public void UpdateStaminaFill(float percentageAmount)
    {
        staminaFill.fillAmount = percentageAmount;
    }

    public void UpdateScore(bool[] starsUnlock, int scoreUntilNextStar)
    {
        scoreToReach.text = "First star in : " + scoreUntilNextStar;

        if (starsUnlock == null || starsUnlock.Length == 0) return;

        if(starsUnlock[0])
        {
            scoreToReach.text = "Next star in : " + scoreUntilNextStar + " <color=#F77D1C>(not bonus)</color>";
        }

        SetActivePlayerHudStars(starsUnlock.Length, starsUnlock);
    }

    void SetActivePlayerHudStars(int checkLength, bool[] states)
    {
        int length = checkLength > nextStars.Length ? checkLength : nextStars.Length;
        for (int i = 0; i < length; i++)
        {
            nextStars[i].SetActive(states[i]);
        }
    }
}
