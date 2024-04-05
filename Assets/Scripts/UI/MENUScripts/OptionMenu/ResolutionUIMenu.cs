using UnityEngine;
using UnityEngine.UI;
using TMPro;

using GraphicsOption;

namespace UI.MENUScripts.Options
{
    public class ResolutionUIMenu : MonoBehaviour
    {
        [SerializeField] Toggle m_SetFullScreenMode;
        [SerializeField] TMP_Text m_ResolutionText;
        [SerializeField] Button m_LeftButtonResolution;
        [SerializeField] Button m_RightButtonResolution;
        [SerializeField] Button m_ApplyGraphicsParameters;

        void Start()
        {
            m_SetFullScreenMode.SetIsOnWithoutNotify(PlayerPrefs.GetInt("is_fullscreen", 0) != 0);
            BindUIInteractables();
            ResetUIToCurrentAppliedInfos();
        }

        void BindUIInteractables()
        {
            m_SetFullScreenMode.onValueChanged.AddListener(GraphicsOptionManagement.s_Instance.SetFullScreenMode);
            m_LeftButtonResolution.onClick.AddListener(() => {SwitchToNextResolution(-1);});
            m_RightButtonResolution.onClick.AddListener(() => {SwitchToNextResolution(1);});
            
            m_SetFullScreenMode.onValueChanged.AddListener(GraphicsOptionManagement.s_Instance.SetFullScreenMode);

            m_ApplyGraphicsParameters.onClick.AddListener(GraphicsOptionManagement.s_Instance.ApplyGraphicsOptions);

        }

        void SwitchToNextResolution(int dir)
        {
            GraphicsOptionManagement.s_Instance.SwitchToNextResolution(dir);
            m_ResolutionText.text = GraphicsOptionManagement.s_Instance.GetSelectedResolutionString();
        }

        public void ResetUIToCurrentAppliedInfos()
        {
            m_ResolutionText.text = GraphicsOptionManagement.s_Instance.GetAppliedResolutionString();
        }
    }
}