using TimeOption;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ControlOptions;
using Audio;

namespace UI.MENUScripts
{
    public class PauseMenuUI : MonoBehaviour
    {
        public Transform optionMenuParent;
        OptionMenu optionMenu;
        [SerializeField] Button optionMenuButton;
        [SerializeField] Button continueButton;
        [SerializeField] Button quitButton;
        [SerializeField] LevelData targetedQuitLevel;
        [SerializeField] Button restartButton;
        bool blockPauseMenuFromOpening = false;

        public void Init(UIManager uIManager)
        {
            optionMenu = uIManager.optionMenu;

            if(optionMenuButton)
                optionMenuButton.onClick.AddListener(ToggleOptionMenu);

            if(continueButton)
                continueButton.onClick.AddListener(ToggleActiveMenuState);

            if (quitButton)
            {
                quitButton.onClick.AddListener(() =>
                {
                    AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_QuitMenuSound);
                    GoToLevel(targetedQuitLevel.linkedScenePath);
                });
            }

            if(restartButton)
                restartButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }

        public void ToggleActiveMenuState()
        {
            if(blockPauseMenuFromOpening)
            {
                return;
            }

            ToggleMenu();
        }

        void ToggleMenu()
        {
            bool state = !gameObject.activeSelf;

            bool canHandleMenuLayer = HandleMenuLayer();

            if (state || canHandleMenuLayer)
            {
                ControlOptionsManagement.s_Instance.UpdateIsMainControlsActivated(!state);

                TimeOptionManagement.SetActiveTime(!state);

                gameObject.SetActive(state);
                ControlOptionsManagement.SetCursorIsPlayMode(!state);

                optionMenu.SetActiveOptionMenu(false);
            }
        }

        public void SetBlockPauseMenu(bool state, bool close)
        {
            if(gameObject.activeSelf && close)
            {
                ToggleMenu();
            }
            blockPauseMenuFromOpening = state;
        }

        bool HandleMenuLayer()
        {
            return !optionMenu.HandleMenuLayer();
        }

        void GoToLevel(string scenePath)
        {
            LevelLoader.s_instance.LoadLevel(scenePath);
        }

        public void ToggleOptionMenu()
        {
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_OpenUiMenu);
            optionMenu.ToggleOptionMenu();
        }

        public void OnButtonHover()
        {
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_HoverUIButtons);
        }
    }
}