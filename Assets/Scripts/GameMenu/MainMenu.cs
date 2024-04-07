using Manager;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : LevelManager
{
    [SerializeField] Button m_LevelSelectorBtn;
    [SerializeField] Button m_OptionBtn;

    [Header("Level Selector")]
    [SerializeField] GameObject m_LevelSelectorPanel;
    [SerializeField] LevelButtons[] m_Levels;

    [Serializable]
    class LevelButtons
    {
        public Button m_LevelButton;
        public LevelData m_LevelData;
    }

    void Start()
    {
        LoadLevelSelectorBtn();
        m_OptionBtn.onClick.AddListener(() => { m_UiManager.optionMenu.ToggleOptionMenu(); });
    }

    void LoadLevelSelectorBtn()
    {
        int levelReached = SaveSystem.GetLevelReached();

        m_LevelSelectorPanel.SetActive(false);
        m_LevelSelectorBtn.onClick.AddListener(() => { m_LevelSelectorPanel.SetActive(!m_LevelSelectorPanel.activeSelf); });

        for (int i=0; i< m_Levels.Length; i++)
        {
            int iCopy = i;
            m_Levels[i].m_LevelButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(m_Levels[iCopy].m_LevelData.linkedScenePath));

            m_Levels[i].m_LevelButton.interactable = i > levelReached ? false : true;
        }
    }
}