using UnityEngine;
using UnityEngine.UI;

namespace UI.MENUScripts
{
    public class PauseMenuUI : MonoBehaviour
    {
        OptionMenu optionMenu;
        [SerializeField] Button optionMenuButton;

        public void Init(UIManager uIManager)
        {
            optionMenu = uIManager.optionMenu;

            optionMenuButton.onClick.AddListener(ToggleOptionMenu);
        }

        public void ToggleActiveMenuState()
        {
            bool state = !gameObject.activeSelf;
            gameObject.SetActive(state);
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;

            optionMenu.SetActiveOptionMenu(false);
        }

        public void QuitButton(string sceneName)
        {
            LevelLoader.s_instance.LoadLevel(sceneName);
        }

        public void ToggleOptionMenu()
        {
            optionMenu.ToggleOptionMenu();
        }
    }
}