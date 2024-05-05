using Manager;
using PlayerSystems.PlayerInput;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : LevelManager
{
    [SerializeField] GameObject m_DefaultAudioSelectionMenu;

    [SerializeField] Button m_LevelSelectorBtn;
    [SerializeField] Button m_OptionBtn;
    [SerializeField] Button m_QuitBtn;

    [SerializeField] InputManager m_InputManager;

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
        m_InputManager.BindTogglePauseMenu(() => { m_UiManager.optionMenu.HandleMenuLayer(); });

        LoadLevelSelectorBtn();
        m_OptionBtn.onClick.AddListener(() => { m_UiManager.optionMenu.ToggleOptionMenu(); });

        m_QuitBtn.onClick.AddListener(Application.Quit);

        LoadDefaultMusicVolumeSelection();
    }

    void LoadDefaultMusicVolumeSelection()
    {
        m_DefaultAudioSelectionMenu.SetActive(false);
        if (PlayerPrefs.GetInt("DEFAULT_VOLUME_SELECTION_LOCKED", 1) == 1)
        {
            m_DefaultAudioSelectionMenu.SetActive(true);
        }
    }

    public void CloseDefaultVolumeSelectionMenu()
    {
        m_DefaultAudioSelectionMenu.SetActive(false);
        PlayerPrefs.SetInt("DEFAULT_VOLUME_SELECTION_LOCKED", 0);
    }

    void LoadLevelSelectorBtn()
    {
        int levelReached = SaveSystem.GetSavedData().m_LevelReached;

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