using Audio;
using Manager;
using PlayerSystems.PlayerInput;
using System;
using TMPro;
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
    [SerializeField] Image m_MapPreview; 
    [SerializeField] TMP_Text m_LevelDesciption;
    [SerializeField] Sprite[] m_StarSprites;

    [Serializable]
    class LevelButtons
    {
        public Button m_LevelButton;
        public LevelData m_LevelData;
        public Image m_Stars;
    }

    void Start()
    {
        m_InputManager.BindTogglePauseMenu(() => { 
            m_UiManager.optionMenu.HandleMenuLayer();
        });

        LoadLevelSelectorBtn();

        m_OptionBtn.onClick.AddListener(() => {
            SetActivePanel(m_UiManager.optionMenu.GetPanelGo(), !m_UiManager.optionMenu.GetPanelGo().activeSelf);
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_OpenUiMenu);
        });

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
        m_LevelSelectorBtn.onClick.AddListener(() => { 
            SetActivePanel(m_LevelSelectorPanel, !m_LevelSelectorPanel.activeSelf);
        });

        SaveData save = SaveSystem.GetSavedData();
        for (int i=0; i< m_Levels.Length; i++)
        {
            int iCopy = i;
            m_Levels[i].m_LevelButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(m_Levels[iCopy].m_LevelData.linkedScenePath));

            m_Levels[i].m_LevelButton.interactable = i > levelReached ? false : true;

            if (save.m_LevelUnlockedStarsNumber != null && save.m_LevelUnlockedStarsNumber.Length > i)
            {
                if (save.m_LevelUnlockedStarsNumber[i] > m_StarSprites.Length)
                {
                    save.m_LevelUnlockedStarsNumber[i] = m_StarSprites.Length;
                }

                m_Levels[i].m_Stars.sprite = m_StarSprites[save.m_LevelUnlockedStarsNumber[i]];
            }
        }
    }

    public void SetHoverPreview(LevelData levelData)
    {
        m_MapPreview.sprite = levelData.mapPreviewIcon;
        m_LevelDesciption.text = levelData.levelDescription;
        AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_HoverUIButtons);

        SaveData save = SaveSystem.GetSavedData();
        if(levelData.levelID < save.m_LevelHighScores.Length)
        {
            m_LevelDesciption.text += $"\n\nScore : {save.m_LevelHighScores[levelData.levelID]}";
        }
        else
        {
            m_LevelDesciption.text += "\n\nScore : No Registered Score";
        }
    }

    public void OnHoverUI()
    {
        AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_HoverUIButtons);
    }

    void SetActivePanel(GameObject panelToActive, bool state)
    {
        CloseAllPanel();
        panelToActive.SetActive(state);
    }

    void CloseAllPanel()
    {
        m_UiManager.optionMenu.SetActiveOptionMenu(false);
        m_LevelSelectorPanel.SetActive(false);
    }
}