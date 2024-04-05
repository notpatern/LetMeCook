using Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : LevelManager
{
    [SerializeField] Button m_LevelSelectorBtn;
    [SerializeField] Button m_OptionBtn;

    [Header("Level Selector")]
    [SerializeField] GameObject m_LevelSelectorPanel;
    [SerializeField] Button[] m_Levels;
    [SerializeField] int m_FirstLevelBuildIndex;

    void Start()
    {
        LoadLevelSelectorBtn();
        m_OptionBtn.onClick.AddListener(() => { m_UiManager.optionMenu.ToggleOptionMenu(); });
    }

    void LoadLevelSelectorBtn()
    {
        SaveData data = SaveSystem.Load();
        int levelReached = data.m_LevelReached;

        m_LevelSelectorPanel.SetActive(false);
        m_LevelSelectorBtn.onClick.AddListener(() => { m_LevelSelectorPanel.SetActive(!m_LevelSelectorPanel.activeSelf); });

        for (int i=0; i< m_Levels.Length; i++)
        {
            int iCopy = i;
            m_Levels[i].onClick.AddListener(() => { LevelLoader.s_instance.LoadLevel(m_FirstLevelBuildIndex+iCopy); });

            m_Levels[i].interactable = i > levelReached ? false : true;
        }
    }
}
