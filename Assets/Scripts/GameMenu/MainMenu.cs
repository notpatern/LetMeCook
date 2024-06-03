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
    [SerializeField] GameObject[] m_Stars;
    [SerializeField] Sprite[] m_StarSprites;

    [Serializable]
    class LevelButtons
    {
        public Button m_LevelButton;
        public LevelData m_LevelData;
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
            
            //Set star amount
            if (save.m_LevelUnlockedStarsNumber != null && i+1 < save.m_LevelUnlockedStarsNumber.Length)
            {
                print($"lvl {i+1}");
                print($"m_LevelUnlockedStarsNumber {i+1} : " + save.m_LevelUnlockedStarsNumber[i+1]);
                print($"m_LevelHighScores {i+1} : " + save.m_LevelHighScores[i+1]);
                m_Stars[i].GetComponent<Image>().sprite = m_StarSprites[save.m_LevelUnlockedStarsNumber[i]];
            }
        }
    }

    public void SetHoverPreview(LevelData levelData)
    {
        m_MapPreview.sprite = levelData.mapPreviewIcon;
        m_LevelDesciption.text = levelData.levelDescription;
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