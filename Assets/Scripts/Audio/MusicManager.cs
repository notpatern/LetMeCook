using FMOD.Studio;
using System.Collections;
using UnityEngine;

namespace Audio {
    public class MusicManager
    {
        EventInstance m_BackMusicInsatnce;
        LevelMusicData m_LevelMusicData;
        string musicTypeParameter = "MusicTypeValue";
        Coroutine m_MusicTransitionCoroutine;

        public void InitializeMusic(LevelMusicData levelMusicData, MonoBehaviour mono)
        {
            m_LevelMusicData = levelMusicData;
            m_BackMusicInsatnce = AudioManager.s_Instance.CreateInstance(m_LevelMusicData.m_BackMusic);
            m_BackMusicInsatnce.start();
            mono.StartCoroutine(MusicTimeStampChangeValue(mono));
        }

        public void ChangeMusicTypeAmount(float amount)
        {
            if(amount > m_LevelMusicData.m_backMusicTimestamps.Length) {
                amount = m_LevelMusicData.m_backMusicTimestamps.Length;
            }

            m_BackMusicInsatnce.setParameterByName(musicTypeParameter, amount);
        }

        IEnumerator MusicTypeValueTransition(float startAmount, float finalAmount)
        {
            float timer = 0;
            while (timer <= m_LevelMusicData.m_BackMusicTransition)
            {
                ChangeMusicTypeAmount(Lerp(startAmount, finalAmount, timer / m_LevelMusicData.m_BackMusicTransition));

                timer += Time.deltaTime;

                yield return null;
            }

            m_MusicTransitionCoroutine = null;
        }

        float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        IEnumerator MusicTimeStampChangeValue(MonoBehaviour mono)
        {
            for(int i=0; i<m_LevelMusicData.m_backMusicTimestamps.Length; i++)
            {
                yield return new WaitForSeconds(m_LevelMusicData.m_backMusicTimestamps[i]);
                if (m_MusicTransitionCoroutine == null)
                {
                    m_MusicTransitionCoroutine = mono.StartCoroutine(MusicTypeValueTransition(i, i + 1));
                }
            }
        }
    }

}