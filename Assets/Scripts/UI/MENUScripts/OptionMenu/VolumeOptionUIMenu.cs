using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MENUScripts.Options
{
    public class VolumeOptionUIMenu : MonoBehaviour
    {
        [SerializeField] Slider m_MasterVolume;
        [SerializeField] Slider m_MusicVolume;
        [SerializeField] Slider m_SFXVolume;

        private void Start()
        {
            m_MasterVolume.value = AudioManager.s_Instance.m_MasterVolume;
            m_MusicVolume.value = AudioManager.s_Instance.m_MusicVolume;
            m_SFXVolume.value = AudioManager.s_Instance.m_SFXVolume;
        }

        public void SetMasterVolume(float volume)
        {
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_SliderUI);

            AudioManager.s_Instance.SetMasterVolume(volume);
            m_MasterVolume.value = volume;
        }

        public void SetMusicVolume(float volume)
        {
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_SliderUI);

            AudioManager.s_Instance.SetMusicVolume(volume);
            m_MusicVolume.value = volume;
        }

        public void SetSFXVolume(float volume)
        {
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_SliderUI);

            AudioManager.s_Instance.SetSFXVolume(volume);
            m_SFXVolume.value = volume;
        }
    }
}