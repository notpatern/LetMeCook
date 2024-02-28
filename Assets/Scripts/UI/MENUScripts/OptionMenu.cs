using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

namespace UI.MENUScripts
{
    public class OptionMenu : MonoBehaviour
    {
        //TODO put OptionSystem as singleton and link optionMenu with only front-end
        //Fullscreen--------
        bool isFullScreen = false;
        //------------------

        //Resolutions-------
        int m_currentSelectedResolutionID;
        Resolution[] m_deviceAvailableResolutions;
        float[] m_authorizedRatios = {16/9, 4/3};
        //------------------

        [Header("Buttons")]
        [SerializeField] Button m_LeftButtonResolution;
        [SerializeField] Button m_RightButtonResolution;
        [SerializeField] Toggle m_SetFullScreenMode;
        [SerializeField] Button m_GraphicsParameter;
        [SerializeField] TMP_Text m_ResolutionText;
        
        [Header("Menus")]
        [SerializeField] GameObject m_GraphicsPanel;

        void Awake()
        {
            m_GraphicsPanel.SetActive(false);
            //SetDefaultValues
            LoadResolution();

            m_SetFullScreenMode.SetIsOnWithoutNotify(PlayerPrefs.GetInt("is_fullscreen", 0) != 0);
            
            m_currentSelectedResolutionID = PlayerPrefs.GetInt("current_resolution_id", m_currentSelectedResolutionID);

            UpdateResolution();

            //SetListener
            m_LeftButtonResolution.onClick.AddListener(() => {SwitchToNextResolution(-1);});
            m_RightButtonResolution.onClick.AddListener(() => {SwitchToNextResolution(1);});
            m_SetFullScreenMode.onValueChanged.AddListener(SetFullScreenMode);
            m_GraphicsParameter.onClick.AddListener(ToggleGraphicsParameter);

            m_GraphicsPanel.SetActive(false);
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
                        m_currentSelectedResolutionID = currentResolutionId-1;
                    }
                }
            }

            Array.Resize(ref m_deviceAvailableResolutions, currentResolutionId);

        }
        
        bool IsResolutionsSameSize(Resolution a, Resolution b)
        {
            return a.width == b.width && a.height == b.height;
        }

        bool IsAuthorizedRatio(Resolution a)
        {
            foreach(int ration in m_authorizedRatios)
            {
                if(ration == a.width / a.height)
                    return true;
            }
            
            return false;
        }

        void SwitchToNextResolution(int dir)
        {
            m_currentSelectedResolutionID += dir;
            if(m_currentSelectedResolutionID < 0)
            {
                m_currentSelectedResolutionID = m_deviceAvailableResolutions.Length-1;
            }
            else if(m_currentSelectedResolutionID > m_deviceAvailableResolutions.Length-1)
            {
                m_currentSelectedResolutionID = 0;
            }

            UpdateResolution();
        }

        void UpdateResolution()
        {
            if(m_currentSelectedResolutionID >= m_deviceAvailableResolutions.Length)
            {
                m_currentSelectedResolutionID = m_deviceAvailableResolutions.Length-1;
            }

            Screen.SetResolution(m_deviceAvailableResolutions[m_currentSelectedResolutionID].width, m_deviceAvailableResolutions[m_currentSelectedResolutionID].height, isFullScreen);
            m_ResolutionText.text = ((int)m_deviceAvailableResolutions[m_currentSelectedResolutionID].refreshRateRatio.value).ToString() + "fps | " + m_deviceAvailableResolutions[m_currentSelectedResolutionID].width + "x" + m_deviceAvailableResolutions[m_currentSelectedResolutionID].height;
            
            PlayerPrefs.SetInt("is_fullscreen", isFullScreen ? 1 : 0);
            PlayerPrefs.SetInt("current_resolution_id", m_currentSelectedResolutionID);
        }

        public void SetFullScreenMode(bool state)
        {
            isFullScreen = state;
            UpdateResolution();
        }

        void ToggleGraphicsParameter()
        {
            m_GraphicsPanel.SetActive(!m_GraphicsPanel.activeSelf);
        }

        void ConfirmGraphicsParameters()
        {
            
        }
    }
}