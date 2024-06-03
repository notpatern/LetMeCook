using UnityEngine;
using System;
using Audio;

namespace GraphicsOption
{
    public class GraphicsOptionManagement
    {
        public static GraphicsOptionManagement s_Instance;

        //Fullscreen--------
        bool isFullScreen = false;
        //------------------
        //Resolutions-------
        int m_currentSelectedResolutionID;
        int m_DirtySelectedResolutionID;
        Resolution[] m_deviceAvailableResolutions;
        float[] m_authorizedRatios = {16f/9f, 4f/3f, 16f/10f};
        //------------------

        public static void LoadGraphicsOptionManagement()
        {
            if(s_Instance == null)
            {
                s_Instance = new GraphicsOptionManagement();
                s_Instance.LoadGraphicOptions();
            }
        }

        void LoadGraphicOptions()
        {
            LoadResolution();
            isFullScreen = PlayerPrefs.GetInt("is_fullscreen", 1) != 0 ? true : false;
            m_currentSelectedResolutionID = PlayerPrefs.GetInt("current_resolution_id", m_currentSelectedResolutionID);
            m_DirtySelectedResolutionID = m_currentSelectedResolutionID;

            UpdateResolution();
        }

        public void ApplyGraphicsOptions()
        {
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_SwitchResolutionAndToggle);

            UpdateResolution();
        }

        public string GetSelectedResolutionString()
        {
            return GetResolutionString(m_deviceAvailableResolutions[m_DirtySelectedResolutionID]);
        }

        public string GetAppliedResolutionString()
        {
            return GetResolutionString(m_deviceAvailableResolutions[m_currentSelectedResolutionID]);
        }

        string GetResolutionString(in Resolution resolution)
        {
            // return ((int)resolution.refreshRateRatio.value).ToString() + "fps | " + resolution.width + "x" + resolution.height;
            return resolution.width + "x" + resolution.height;
        }

        void LoadResolution()
        {
            m_deviceAvailableResolutions = new Resolution[Screen.resolutions.Length];

            int currentResolutionId = -1;
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (IsAuthorizedRatio(Screen.resolutions[i]))
                {
                    currentResolutionId++;
                    m_deviceAvailableResolutions[currentResolutionId].width = Screen.resolutions[i].width;
                    m_deviceAvailableResolutions[currentResolutionId].height = Screen.resolutions[i].height;
                }
            }

            Array.Resize(ref m_deviceAvailableResolutions, currentResolutionId + 1);

            m_currentSelectedResolutionID = m_deviceAvailableResolutions.Length - 1;

            for (int i = 0; i < m_deviceAvailableResolutions.Length; i++)
            { 
                if (m_deviceAvailableResolutions[i].width == Screen.width && m_deviceAvailableResolutions[i].width == Screen.height)
                {
                    m_currentSelectedResolutionID = i;
                }
            }
        }
        
        bool IsResolutionsSameSize(in Resolution a, in Resolution b)
        {
            return a.width == b.width && a.height == b.height;
        }

        public void SetFullScreenMode(bool state)
        {
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_SwitchResolutionAndToggle);
            isFullScreen = state;
        }

        bool IsAuthorizedRatio(in Resolution a)
        {
            if(a.width == 0 || a.height == 0) return false;

            foreach(ulong ratio in m_authorizedRatios)
            {
                //Debug.Log(ratio + " | width " + a.width + " | height" + a.height + "refresh : " + a.refreshRateRatio.value + " | refresh current : " + Screen.currentResolution.refreshRateRatio.value);
                if (ratio == (ulong)a.width / (ulong)a.height && a.refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value)
                {
                    return true;
                }
            }
            
            return false;
        }

        public void SwitchToNextResolution(int dir)
        {
            m_DirtySelectedResolutionID += dir;
            if(m_DirtySelectedResolutionID < 0)
            {
                m_DirtySelectedResolutionID = m_deviceAvailableResolutions.Length-1;
            }
            else if(m_DirtySelectedResolutionID > m_deviceAvailableResolutions.Length-1)
            {
                m_DirtySelectedResolutionID = 0;
            }
        }

        void UpdateResolution()
        {
            m_currentSelectedResolutionID = m_DirtySelectedResolutionID;
            if(m_currentSelectedResolutionID >= m_deviceAvailableResolutions.Length)
            {
                m_currentSelectedResolutionID = m_deviceAvailableResolutions.Length-1;
            }

            Screen.SetResolution(m_deviceAvailableResolutions[m_currentSelectedResolutionID].width, m_deviceAvailableResolutions[m_currentSelectedResolutionID].height, isFullScreen);
            PlayerPrefs.SetInt("is_fullscreen", isFullScreen ? 1 : 0);
            
            PlayerPrefs.SetInt("current_resolution_id", m_currentSelectedResolutionID);
        }
    }
}