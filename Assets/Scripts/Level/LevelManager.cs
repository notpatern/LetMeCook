using UI;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] DefaultLevelData m_DefaultLevelData;
        protected UIManager m_UiManager;
        [SerializeField] LevelData m_LevelData;

        protected virtual void Awake()
        {
            if(!LevelLoader.s_instance)
            {
                Instantiate(m_DefaultLevelData.LevelLoader);
            }

            GraphicsOption.GraphicsOptionManagement.LoadGraphicsOptionManagement();
            ControlOptions.ControlOptionsManagement.LoadControlOptionsManagement();
            TimeOption.TimeOptionManagement.LoadTimeOptionsManagement();

            m_UiManager = new UIManager();
            m_UiManager.LoadUI(m_LevelData.levelUIData, m_LevelData.dialogLevelData);
        }
    }
}