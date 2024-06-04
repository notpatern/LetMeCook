using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName ="AudioSoundData", menuName ="LetMeCook/Audio/AudioSoundData")]
public class AudioSoundData : ScriptableObject
{
    [Header("PlayerFoodAction")]
    public EventReference m_PlayerThrowSound;
    public EventReference m_PlayerPickFood;

    [Header("PlayerMovement")]
    public EventReference m_PlayerGroundImpact;
    public EventReference m_PlayerJump;
    public EventReference m_PlayerWallrun;
    public EventReference m_PlayerDoubleJump;
    public EventReference m_PlayerDash;
    public EventReference m_PlayerWindSpeed;

    [Header("Food")]
    public EventReference m_FoodBounceImpact;
    public EventReference m_FoodAirThrowingEffect;
    public EventReference m_LevelTimeRemainingWarning;
    //public EventReference m_RecipeTimeRemainingWarning;

    [Header("Transformator")]
    public EventReference m_FoodInCookingRange;
    public EventReference m_BakerCooking;
    public EventReference m_ChopperCooking;
    public EventReference m_PurifierCooking;

    [Header("MouthGoal")]
    public EventReference m_DeliverySound;
    public EventReference m_DeliverySoundFailure;

    [Header("UI")]
    public EventReference m_OpenUiMenu;
    public EventReference m_HoverUIButtons;
    public EventReference m_PauseAndOptionMenuButton;
    public EventReference m_QuitMenuSound;
    public EventReference m_SliderUI;
    public EventReference m_SwitchResolutionAndToggle;
    public EventReference m_StartLevelButton;
}
