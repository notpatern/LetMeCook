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
            SetMasterVolume(PlayerPrefs.GetFloat("VOLUME_MASTER", 0.5f));
            SetMusicVolume(PlayerPrefs.GetFloat("VOLUME_MUSIC", 0.5f));
            SetSFXVolume(PlayerPrefs.GetFloat("VOLUME_SFX", 0.5f));

            m_MasterVolume.value = AudioManager.s_Instance.m_MasterVolume;
            m_MusicVolume.value = AudioManager.s_Instance.m_MusicVolume;
            m_SFXVolume.value = AudioManager.s_Instance.m_SFXVolume;
        }

        public void SetMasterVolume(float volume)
        {
            PlayerPrefs.SetFloat("VOLUME_MASTER", volume);
            AudioManager.s_Instance.SetMasterVolume(volume);
            m_MasterVolume.value = volume;
        }

        public void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat("VOLUME_MUSIC", volume);
            AudioManager.s_Instance.SetMusicSolume(volume);
            m_MusicVolume.value = volume;
        }

        public void SetSFXVolume(float volume)
        {
            PlayerPrefs.SetFloat("VOLUME_SFX", volume);
            AudioManager.s_Instance.SetSFXVolume(volume);
            m_SFXVolume.value = volume;
        }
    }
}