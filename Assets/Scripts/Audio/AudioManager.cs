using UnityEngine;
using FMODUnity;

namespace Audio
{
    public class AudioManager
    {
        public static AudioManager s_Instance { get; private set; }
        public AudioSoundData m_AudioSoundData;

        public static void InitAudioManager(AudioSoundData audioSoundData)
        {
            if(s_Instance == null)
            {
                s_Instance = new AudioManager();
            }

            s_Instance.m_AudioSoundData = audioSoundData;
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPosition)
        {
            RuntimeManager.PlayOneShot(sound, worldPosition);
        }
    }

}