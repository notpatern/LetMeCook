using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

namespace Audio
{
    public class AudioManager
    {
        List<EventInstance> m_EventInstances;
        public static AudioManager s_Instance { get; private set; }
        public AudioSoundData m_AudioSoundData;

        public static void InitAudioManager(AudioSoundData audioSoundData)
        {
            if(s_Instance == null)
            {
                s_Instance = new AudioManager();
                s_Instance.m_EventInstances = new List<EventInstance>();
            }

            s_Instance.m_AudioSoundData = audioSoundData;
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPosition)
        {
            RuntimeManager.PlayOneShot(sound, worldPosition);
        }

        EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance instance = RuntimeManager.CreateInstance(eventReference);
            m_EventInstances.Add(instance);
            return instance;
        }

        void CleanUp()
        {
            foreach(EventInstance instance in m_EventInstances)
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
            }
        }
    }

}