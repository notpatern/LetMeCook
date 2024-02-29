using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        int scenesCount = SceneManager.sceneCountInBuildSettings;

        string currentSceneName;
        for(int i=0; i < scenesCount; i++)
        {
            currentSceneName = System.IO.Path.GetFileNameWithoutExtension( SceneUtility.GetScenePathByBuildIndex( i ) );
            if (sceneName.ToLower() == currentSceneName.ToLower())
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
            yield return null;
        } 
        gameObject.SetActive(false);
    }
}
