using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

namespace Audio
{
    public class AudioManager
    {
        List<EventInstance> m_PausableEventInstances;
        List<EventInstance> m_UnpausableEventInstances;
        public static AudioManager s_Instance { get; private set; }
        public AudioSoundData m_AudioSoundData;

        [HideInInspector] public float m_MasterVolume = 1;
        [HideInInspector] public float m_MusicVolume = 1;
        [HideInInspector] public float m_SFXVolume = 1;
        [HideInInspector] public float m_VoiceVolume = 1;

        [HideInInspector] public Bus m_MasterBus;
        [HideInInspector] public Bus m_MusicBus;
        [HideInInspector] public Bus m_SFXBus;
        [HideInInspector] public Bus m_VoiceBus;

        PLAYBACK_STATE m_RuntimeClearCheckerPbState;

        BoolTable m_IsPaused;
        bool m_DirtyPausedState;

        public AudioManager()
        {
            m_MasterBus = RuntimeManager.GetBus("bus:/");
            m_MusicBus = RuntimeManager.GetBus("bus:/Music");
            m_SFXBus = RuntimeManager.GetBus("bus:/SFX");
            m_VoiceBus = RuntimeManager.GetBus("bus:/Voice");

            SetMasterVolume(PlayerPrefs.GetFloat("VOLUME_MASTER", 0.1f));
            SetMusicVolume(PlayerPrefs.GetFloat("VOLUME_MUSIC", 0.1f));
            SetSFXVolume(PlayerPrefs.GetFloat("VOLUME_SFX", 0.1f));
            SetVoiceVolume(PlayerPrefs.GetFloat("VOLUME_VOICE", 0.1f));
        }

        public static void InitAudioManager(AudioSoundData audioSoundData)
        {
            if(s_Instance == null)
            {
                s_Instance = new AudioManager();
                s_Instance.m_PausableEventInstances = new List<EventInstance>();
                s_Instance.m_UnpausableEventInstances = new List<EventInstance>();
            }

            s_Instance.m_AudioSoundData = audioSoundData;
            s_Instance.m_IsPaused = new BoolTable();
        }

        public void Update()
        {
            CleanStoppedSounds(ref m_PausableEventInstances);
            CleanStoppedSounds(ref m_UnpausableEventInstances);
        }

        void CleanStoppedSounds(ref List<EventInstance> eventReferences)
        {
            for (int i = 0; i < eventReferences.Count; i++)
            {
                eventReferences[i].getPlaybackState(out m_RuntimeClearCheckerPbState);

                if (m_RuntimeClearCheckerPbState == PLAYBACK_STATE.STOPPED)
                {
                    eventReferences[i].release();
                    eventReferences.RemoveAt(i);
                }
            }
        }

        public void SetPause(bool state)
        {
            m_IsPaused.Value = state;

            if (m_IsPaused.Value != m_DirtyPausedState)
            {
                m_DirtyPausedState = m_IsPaused.Value;
                for (int i = 0; i < m_PausableEventInstances.Count; i++)
                {
                    m_PausableEventInstances[i].setPaused(m_IsPaused.Value);
                }
            }
        }

        public void PlayOneShot2D(EventReference sound)
        {
            EventInstance instance = CreateInstance(sound);
            instance.start();
            instance.release();
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPosition)
        {
            EventInstance instance = CreateInstance(sound);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(worldPosition));
            instance.start();
            instance.release();
        }

        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance instance = RuntimeManager.CreateInstance(eventReference);
            m_PausableEventInstances.Add(instance);
            return instance;
        }

        public EventInstance Create3DInstance(EventReference eventReference, Vector3 position)
        {
            EventInstance instance = RuntimeManager.CreateInstance(eventReference);
            m_PausableEventInstances.Add(instance);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            return instance;
        }

        public EventInstance CreateUnpausableInstance(EventReference eventReference)
        {
            EventInstance instance = RuntimeManager.CreateInstance(eventReference);
            m_UnpausableEventInstances.Add(instance);
            return instance;
        }

        public void CleanUp()
        {
            for(int i=0; i < m_PausableEventInstances.Count; i++)
            {
                m_PausableEventInstances[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                m_PausableEventInstances[i].release();
            }

            for (int i = 0; i < m_UnpausableEventInstances.Count; i++)
            {
                m_UnpausableEventInstances[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                m_UnpausableEventInstances[i].release();
            }
        }

        public void SetMasterVolume(float volume)
        {
            PlayerPrefs.SetFloat("VOLUME_MASTER", volume);
            m_MasterVolume = volume;
            m_MasterBus.setVolume(volume);
        }

        public void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat("VOLUME_MUSIC", volume);
            m_MusicVolume = volume;
            m_MusicBus.setVolume(volume);
        }

        public void SetSFXVolume(float volume)
        {
            PlayerPrefs.SetFloat("VOLUME_SFX", volume);
            m_SFXVolume = volume;
            m_SFXBus.setVolume(volume);
        }

        public void SetVoiceVolume(float volume)
        {
            PlayerPrefs.SetFloat("VOLUME_VOICE", volume);
            m_VoiceVolume = volume;
            m_VoiceBus.setVolume(volume);
        }
}

}