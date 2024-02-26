using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.MENUScripts
{
    public class PauseMenuUI : MonoBehaviour
    {
        void ToggleActiveMenuState()
        {
            bool state = !gameObject.activeSelf;
            gameObject.SetActive(state);
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;
        }

        void QuitButton(string sceneName)
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            sceneName.ToLower();
            foreach (EditorBuildSettingsScene scene in scenes)
            {
                string currentSceneName = scene.path;
                currentSceneName = currentSceneName.Substring(currentSceneName.LastIndexOf('\\') + 1);
                currentSceneName = currentSceneName.Substring(0, currentSceneName.Length - 6);
                if (sceneName == currentSceneName)
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
        }
    }
}