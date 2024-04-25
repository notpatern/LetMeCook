using FMODUnity;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMusicData", menuName = "LetMeCook/LevelData/LevelMusicData")]
public class LevelMusicData : ScriptableObject
{
    public EventReference m_BackMusic;
    public MusicPeriod[] m_MusicPeriod;

    [Serializable]
    public class MusicPeriod
    {
        public bool m_IsOneShot = false;
        [Header("not usefull if m_IsOneShot true")]
        public float m_MusicLoopPeriodDuration = 10f;
    }
}
