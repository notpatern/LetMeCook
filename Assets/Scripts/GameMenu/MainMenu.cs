using Manager;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : LevelManager
{
    [SerializeField] Button m_StoryModeBtn;
    [SerializeField] Button m_LevelSelectorBtn;
    [SerializeField] Button m_OptionBtn;

    void Start()
    {
        m_StoryModeBtn.onClick.AddListener(() => {LevelLoader.s_instance.LoadLevel("storymode");});
        m_LevelSelectorBtn.onClick.AddListener(() => {LevelLoader.s_instance.LoadLevel("levelselector");});
        m_OptionBtn.onClick.AddListener(() => { m_UiManager.optionMenu.ToggleOptionMenu(); });
    }
}
