using UnityEngine;
using UnityEngine.UI;

namespace UI.MENUScripts
{
    public class PauseMenuUI : MonoBehaviour
    {
        GameObject optionMenuGo;
        [SerializeField] Button optionMenuButton;

        public void Init(UIManager uIManager)
        {
            optionMenuGo = uIManager.optionMenu.gameObject;

            optionMenuButton.onClick.AddListener(ToggleOptionMenu);
        }

        public void ToggleActiveMenuState()
        {
            bool state = !gameObject.activeSelf;
            gameObject.SetActive(state);
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;

            optionMenuGo.SetActive(false);
        }

        public void QuitButton(string sceneName)
        {
            LevelLoader.instance.LoadLevel(sceneName);
        }

        public void ToggleOptionMenu()
        {
            optionMenuGo.SetActive(!optionMenuGo.activeSelf);
        }
    }
}