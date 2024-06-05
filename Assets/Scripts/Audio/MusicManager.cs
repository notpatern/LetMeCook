using FMOD.Studio;
using System;
using System.Collections;
using UnityEngine;

namespace Audio {
    public class MusicManager
    {
        EventInstance m_BackMusicInsatnce;
        LevelMusicData m_LevelMusicData;
        string musicTypeParameter = "MusicTypeValue";
        int currentMusicPeriodAmount;
        EVENT_CALLBACK markerCallback;
        MonoBehaviour m_Mono;
        bool m_IsMusicPeriodPlaying;
        int m_CurrentMusicPeriodOffset = 0;

        public void InitializeMusic(LevelMusicData levelMusicData, MonoBehaviour mono)
        {
            m_Mono = mono;
            markerCallback = new EVENT_CALLBACK(MarkerEventCallback);
            m_LevelMusicData = levelMusicData;
            m_BackMusicInsatnce = AudioManager.s_Instance.CreateUnpausableInstance(m_LevelMusicData.m_BackMusic);
            m_BackMusicInsatnce.setCallback(markerCallback, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
            m_BackMusicInsatnce.start();
        }

        public void IncreaseMusicTypeOffsetAmount()
        {
            m_CurrentMusicPeriodOffset++;
            m_BackMusicInsatnce.setParameterByName(musicTypeParameter, currentMusicPeriodAmount + m_CurrentMusicPeriodOffset);
        }

        void IncreaseMusicPeriodCurrentAmount()
        {
            currentMusicPeriodAmount++;
            if (currentMusicPeriodAmount > m_LevelMusicData.m_MusicPeriod.Length - 1)
            {
                currentMusicPeriodAmount = m_LevelMusicData.m_MusicPeriod.Length - 1;
            }

            m_BackMusicInsatnce.setParameterByName(musicTypeParameter, currentMusicPeriodAmount + m_CurrentMusicPeriodOffset);
        }

        IEnumerator MusicTimeStampChangeValue()
        {
            m_IsMusicPeriodPlaying = true;
            float duration;
            LevelMusicData.MusicPeriod currentPeriod;
                
            currentPeriod = m_LevelMusicData.m_MusicPeriod[currentMusicPeriodAmount];
            duration = currentPeriod.m_IsOneShot ? 0: currentPeriod.m_MusicLoopPeriodDuration;
            yield return new WaitForSeconds(duration);
            IncreaseMusicPeriodCurrentAmount();
            m_IsMusicPeriodPlaying = false;
        }

        FMOD.RESULT MarkerEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
        {
            if(type == EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
            {
                if (!m_IsMusicPeriodPlaying)
                {
                    if (m_Mono)
                    {
                        m_Mono.StartCoroutine(MusicTimeStampChangeValue());
                    }
                }
            }

            return FMOD.RESULT.OK;
        }
    }

}