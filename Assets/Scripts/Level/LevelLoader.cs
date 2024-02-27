using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    [SerializeField] Image loadProgressFill;
    void Awake()
    {
        if(!instance)
        {
            instance = this;
            gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool LoadLevel(string sceneName)
    {
        StartLoadingScreen();
        if(CheckSceneName(sceneName))
        {
            StartCoroutine(LoadLevelAsync(StartOperation(sceneName)));
            return true;
        }

        return false;
    }

    public bool LoadLevel(int id)
    {
        StartLoadingScreen();
        StartCoroutine(LoadLevelAsync(StartOperation(id)));

        return true;
    }

    void StartLoadingScreen()
    {
        gameObject.SetActive(true);
    }
    
    AsyncOperation StartOperation(int id)
    {
        return SceneManager.LoadSceneAsync(id);
    }

    AsyncOperation StartOperation(string sceneName)
    {
        return SceneManager.LoadSceneAsync(sceneName);
    }

    bool CheckSceneName(string sceneName)
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
                return true;
            }
        }

        return false;
    }

    IEnumerator LoadLevelAsync(AsyncOperation operation)
    {
        while(!operation.isDone)
        {
            loadProgressFill.fillAmount = operation.progress;
            Debug.Log(operation.progress);
            yield return null;
        } 
        gameObject.SetActive(false);
    }
}
