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

        [HideInInspector] public float m_MasterVolume = 1;
        [HideInInspector] public float m_MusicVolume = 1;
        [HideInInspector] public float m_SFXVolume = 1;

        [HideInInspector] public Bus m_MasterBus;
        [HideInInspector] public Bus m_MusicBus;
        [HideInInspector] public Bus m_SFXBus;
        
        public AudioManager()
        {
            m_MasterBus = RuntimeManager.GetBus("bus:/");
            m_MusicBus = RuntimeManager.GetBus("bus:/Music");
            m_SFXBus = RuntimeManager.GetBus("bus:/SFX");
        }

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

        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance instance = RuntimeManager.CreateInstance(eventReference);
            m_EventInstances.Add(instance);
            return instance;
        }

        public void CleanUp()
        {
            foreach(EventInstance instance in m_EventInstances)
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
            }
        }

        public void SetMasterVolume(float volume)
        {
            m_MasterVolume = volume;
            m_MasterBus.setVolume(volume);
        }

        public void SetMusicSolume(float volume)
        {
            m_MusicVolume = volume;
            m_MusicBus.setVolume(volume);
        }

        public void SetSFXVolume(float volume)
        {
            m_SFXVolume = volume;
            m_SFXBus.setVolume(volume);
        }
    }

}