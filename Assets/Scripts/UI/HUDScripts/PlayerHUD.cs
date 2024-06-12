using Player.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public PlayerInteractionUI playerInteractionUI;
    public Image staminaFill;

    [SerializeField] Image dashImage;
    [SerializeField] Image jumpImage;
    [SerializeField] Image wallImage;

    [SerializeField] Color emptyColor;
    [SerializeField] Color dashColor;
    [SerializeField] Color jumpColor;
    [SerializeField] Color wallColor;

    [SerializeField] GameEventScriptableObject dashEvent;
    [SerializeField] GameEventScriptableObject jumpEvent;
    [SerializeField] GameEventScriptableObject wallEvent;

    [SerializeField] TMP_Text scoreToReach;
    [SerializeField] GameObject[] nextStars;

    private void Awake()
    {
        for (int i = 0; i < nextStars.Length; i++)
        {
            nextStars[i].SetActive(false);
        }

        dashImage.color = emptyColor;
        jumpImage.color = emptyColor;
        wallImage.color = emptyColor;
    }

    private void OnEnable() {
        dashEvent.BindEventAction(EnableDash);
        jumpEvent.BindEventAction(EnableJump);
        wallEvent.BindEventAction(EnableWall);
    }

    private void EnableDash(object args) {
        EnableMovetech(dashImage, dashColor, (bool)args);
    }

    private void EnableJump(object args) {
        EnableMovetech(jumpImage, jumpColor, (bool)args);
    }

    private void EnableWall(object args) {
        EnableMovetech(wallImage, wallColor, (bool)args);
    }

    private void EnableMovetech(Image movetechImage, Color movetechColor, bool state) 
    {
        if (state) {
            movetechImage.color = movetechColor;
            return;
        }
        movetechImage.color = emptyColor;
    }

    public void UpdateStaminaFill(float percentageAmount)
    {
        staminaFill.fillAmount = percentageAmount;
    }

    public void UpdateScore(bool[] starsUnlock, int scoreUntilNextStar)
    {
        scoreToReach.text = "First star in : " + scoreUntilNextStar + " points";

        if (starsUnlock == null || starsUnlock.Length == 0) return;

        if(starsUnlock[0])
        {
            scoreToReach.text = "Next star in : " + scoreUntilNextStar + " points <color=#F77D1C>(without bonus recipes)</color>";
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
