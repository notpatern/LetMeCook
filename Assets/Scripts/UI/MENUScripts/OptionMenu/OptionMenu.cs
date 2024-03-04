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
        
        [Header("Menus")]
        [SerializeField] GameObject m_GraphicsPanel;
        [SerializeField] GameObject m_ControlsPanel;

        [Header("Menu References")]
        [SerializeField] ResolutionUIMenu resolutionUIMenu;

        void Awake()
        {
            m_GraphicsPanel.SetActive(false);
            
            m_GraphicsParameter.onClick.AddListener(ToggleGraphicsParameter);
            m_ControlsParameter.onClick.AddListener(ToggleControlsParameter);

            CloseAllPanel();
        }

        public void ToggleOptionMenu() 
        {
            SetActiveOptionMenu(!gameObject.activeSelf);
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

            if(gameObject.activeSelf)
            {
                ToggleOptionMenu();
                return true;
            }

            return false;
        }

        public void SetActiveOptionMenu(bool state)
        {
            gameObject.SetActive(state);
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
            resolutionUIMenu.ResetUIToCurrentAppliedInfos();
        }

        public void ToggleControlsParameter()
        {
            TogglePanel(m_ControlsPanel);
            
        }

        void TogglePanel(GameObject go)
        {
            CloseAllPanel(go);
            go.SetActive(!go.activeSelf);
        }
    }
}