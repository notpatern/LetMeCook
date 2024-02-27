using UnityEngine;

namespace UI.MENUScripts
{
    public class PauseMenuUI : MonoBehaviour
    {
        public void ToggleActiveMenuState()
        {
            bool state = !gameObject.activeSelf;
            gameObject.SetActive(state);
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;
        }

        public void QuitButton(string sceneName)
        {
            LevelLoader.instance.LoadLevel(sceneName);
        }
    }
}