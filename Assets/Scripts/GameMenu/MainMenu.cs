using Manager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : LevelManager
{
    [SerializeField] Button m_StoryModeBtn;
    [SerializeField] TMP_Text m_StoryModeBtnText;
    [SerializeField] Button m_LevelSelectorBtn;
    [SerializeField] Button m_OptionBtn;

    void Start()
    {
        LoadStoryModeBtn();
        m_LevelSelectorBtn.onClick.AddListener(() => {LevelLoader.s_instance.LoadLevel("levelselector");});
        m_OptionBtn.onClick.AddListener(() => { m_UiManager.optionMenu.ToggleOptionMenu(); });
    }

    void LoadStoryModeBtn()
    {
        int currentLevel = PlayerPrefs.GetInt("LEVEL_REACHED", 0);

        m_StoryModeBtnText.text = currentLevel == 0 ? "New Game" : "Contiue" ;

        m_StoryModeBtn.onClick.AddListener(() => { LevelLoader.s_instance.LoadLevel(SceneManager.GetSceneByName("TestLevel0").buildIndex + currentLevel); });
    }
}
