using UnityEngine;
using System;

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
            RefreshRate defaultResolutionFrameRate = Screen.currentResolution.refreshRateRatio;

            int currentResolutionId = -1;
            for(int i=0; i<Screen.resolutions.Length; i++)
            {
                if(IsAuthorizedRatio(Screen.resolutions[i]) && Screen.resolutions[i].refreshRateRatio.value == defaultResolutionFrameRate.value)
                {
                    currentResolutionId ++;
                    m_deviceAvailableResolutions[currentResolutionId] = Screen.resolutions[i];

                    if(IsResolutionsSameSize(Screen.resolutions[i], Screen.currentResolution))
                    {
                        m_currentSelectedResolutionID = currentResolutionId;
                    }
                }
            }

            Array.Resize(ref m_deviceAvailableResolutions, currentResolutionId+1);

        }
        
        bool IsResolutionsSameSize(in Resolution a, in Resolution b)
        {
            return a.width == b.width && a.height == b.height;
        }

        public void SetFullScreenMode(bool state)
        {
            isFullScreen = state;
        }

        bool IsAuthorizedRatio(in Resolution a)
        {
            if(a.width == 0 || a.height == 0) return false;

            foreach(float ratio in m_authorizedRatios)
            {
                if (Mathf.FloatToHalf(ratio) == Mathf.FloatToHalf(a.width / (float)a.height))
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