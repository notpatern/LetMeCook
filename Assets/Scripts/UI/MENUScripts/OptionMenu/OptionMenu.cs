using UI.MENUScripts.Options;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MENUScripts
{
    public class OptionMenu : MonoBehaviour
    {   
        [Header("Buttons")]
        [SerializeField] Button m_GraphicsParameter;
        [SerializeField] Button m_ControlsParameter;
        [SerializeField] Button m_VolumeParameter;

        [Header("Menus")]
        [SerializeField] GameObject m_GraphicsPanel;
        [SerializeField] GameObject m_ControlsPanel;
        [SerializeField] GameObject m_VolumeOptionPanel;
        [SerializeField] GameObject optionPanel;

        [Header("Menu References")]
        [SerializeField] ResolutionUIMenu m_ResolutionUIMenu;
        [SerializeField] ControlOptionsUIMenu m_ControlOptionUIMenu;

        void Awake()
        {
            m_GraphicsPanel.SetActive(false);
            
            m_GraphicsParameter.onClick.AddListener(ToggleGraphicsParameter);
            m_ControlsParameter.onClick.AddListener(ToggleControlsParameter);
            m_VolumeParameter.onClick.AddListener(ToggleVolumeParameter);

            CloseAllPanel();
        }

        public void ToggleOptionMenu() 
        {
            SetActiveOptionMenu(!IsPanelActive());
        }

        public bool IsPanelActive()
        {
            return optionPanel.activeSelf;
        }

        public GameObject GetPanelGo()
        {
            return optionPanel;
        }

        public bool HandleMenuLayer()
        {
            if(m_ControlsPanel.activeSelf)
            {
                ToggleControlsParameter();
                return true;
            }
            
            if(m_GraphicsPanel.activeSelf)
            {
                ToggleGraphicsParameter();
                return true;
            }

            if(m_VolumeOptionPanel.activeSelf)
            {
                ToggleVolumeParameter();
                return true;
            }

            if(optionPanel.activeSelf)
            {
                ToggleOptionMenu();
                return true;
            }

            return false;
        }

        public void SetActiveOptionMenu(bool state)
        {
            optionPanel.SetActive(state);
            CloseAllPanel();
        }

        void CloseAllPanel(GameObject ignoredPanel = null)
        {
            GameObject[] panels = {m_GraphicsPanel, m_ControlsPanel};

            foreach(GameObject panel in panels)
            {
                if(ignoredPanel && panel == ignoredPanel) continue;

                panel.SetActive(false);
            }
        }

        public void ToggleGraphicsParameter()
        {
            TogglePanel(m_GraphicsPanel);
            m_ResolutionUIMenu.ResetUIToCurrentAppliedInfos();
        }

        public void ToggleControlsParameter()
        {
            TogglePanel(m_ControlsPanel);
        }

        public void ToggleVolumeParameter()
        {
            TogglePanel(m_VolumeOptionPanel);
        }

        void TogglePanel(GameObject go)
        {
            CloseAllPanel(go);
            go.SetActive(!go.activeSelf);
        }
    }
}