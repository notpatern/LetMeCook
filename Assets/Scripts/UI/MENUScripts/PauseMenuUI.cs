using TimeOption;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ControlOptions;

namespace UI.MENUScripts
{
    public class PauseMenuUI : MonoBehaviour
    {
        OptionMenu optionMenu;
        [SerializeField] Button optionMenuButton;
        [SerializeField] Button continueButton;
        [SerializeField] Button quitButton;
        [SerializeField] string targetedQuitLevel;
        [SerializeField] Button restartButton;

        public void Init(UIManager uIManager)
        {
            optionMenu = uIManager.optionMenu;

            if(optionMenuButton)
                optionMenuButton.onClick.AddListener(ToggleOptionMenu);

            if(continueButton)
                continueButton.onClick.AddListener(ToggleActiveMenuState);

            if(quitButton)
                quitButton.onClick.AddListener(() => {GoToLevel(targetedQuitLevel); });

            if(restartButton)
                restartButton.onClick.AddListener(RestartLevel);
        }

        public void ToggleActiveMenuState()
        {
            bool state = !gameObject.activeSelf;

            if (state || HandleMenuLayer())
            {
                TimeOptionManagement.s_Instance.SetActiveTime(!state);

                gameObject.SetActive(state);
                ControlOptionsManagement.SetCursorIsPlayMode(!state);

                optionMenu.SetActiveOptionMenu(false);
            }
        }

        bool HandleMenuLayer()
        {
            return !optionMenu.HandleMenuLayer();
        }

        void RestartLevel()
        {
            TimeOptionManagement.s_Instance.SetActiveTime(true);
            LevelLoader.s_instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }

        void GoToLevel(string sceneName)
        {
            LevelLoader.s_instance.LoadLevel(sceneName);
        }

        public void ToggleOptionMenu()
        {
            optionMenu.ToggleOptionMenu();
        }
    }
}