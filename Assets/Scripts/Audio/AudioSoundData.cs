using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName ="AudioSoundData", menuName ="LetMeCook/Audio/AudioSoundData")]
public class AudioSoundData : ScriptableObject
{
    [Header("PlayerFoodAction")]
    public EventReference m_PlayerThrowSound;
    public EventReference m_PlayerPickFood;
    public EventReference m_PlayerMergeFood;
    public EventReference m_PlayerEmptyMergeFood;
    public EventReference m_PlayerCrunchFood;

    [Header("PlayerMovement")]
    public EventReference m_PlayerGroundImpact;
    public EventReference m_PlayerJump;
    public EventReference m_PlayerWallrun;
    public EventReference m_PlayerDoubleJump;
    public EventReference m_PlayerDash;

    [Header("Food")]
    public EventReference m_FoodBounceImpact;
    public EventReference m_FoodAirThrowingEffect;

    [Header("Transformator")]
    public EventReference m_BakerCooking;
    public EventReference m_ChopperCooking;
    public EventReference m_PurifierCooking;

    [Header("MouthGoal")]
    public EventReference m_DeliverySound;

}
