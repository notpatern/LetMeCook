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

        void ToggleGraphicsParameter()
        {
            TogglePanel(m_GraphicsPanel);
            resolutionUIMenu.ResetUIToCurrentAppliedInfos();
        }

        void ToggleControlsParameter()
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