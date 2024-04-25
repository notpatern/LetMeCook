using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName ="AudioSoundData", menuName ="LetMeCook/Audio/AudioSoundData")]
public class AudioSoundData : ScriptableObject
{
    [Header("Player")]
    public EventReference m_PlayerThrowSound;
}
