using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeOption;
using Audio;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader s_instance;
    [SerializeField] Image loadProgressFill;
    void Awake()
    {
        if(!s_instance)
        {
            s_instance = this;
            gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool LoadLevel(string scenePath)
    {
        return LoadLevel(SceneUtility.GetBuildIndexByScenePath(scenePath));
    }

    public bool LoadLevel(int id)
    {
        AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_StartLevelButton);
        StartLoadingScreen();
        StartCoroutine(LoadLevelAsync(StartOperation(id)));

        return true;
    }

    void StartLoadingScreen()
    {
        TimeOptionManagement.SetActiveTime(true);
        gameObject.SetActive(true);
    }
    
    AsyncOperation StartOperation(int id)
    {
        return SceneManager.LoadSceneAsync(id);
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
