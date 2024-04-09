using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMusicData", menuName = "LetMeCook/LevelData/LevelMusicData")]
public class LevelMusicData : ScriptableObject
{
    public EventReference m_BackMusic;
    public float m_BackMusicTransition;
    public float[] m_backMusicTimestamps;
}
