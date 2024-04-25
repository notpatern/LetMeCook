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

            if(quitButton)
                quitButton.onClick.AddListener(() => { GoToLevel(targetedQuitLevel.linkedScenePath); });

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

            if (state || HandleMenuLayer())
            {
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
            optionMenu.ToggleOptionMenu();
        }
    }
}