using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelData levelData;
    [SerializeField] Player.Player player;
    UIManager uiManager;
    //[SerializeField] string key = "LevelData1";
    //AsyncOperationHandle<LevelData> opHandle;
    void Awake()
    { 
        uiManager = new UIManager();
        uiManager.LoadUI(levelData.levelUIData);
        player.InitUIEvent(uiManager);
        //StartCoroutine(LoadLevelData());
    }

//    IEnumerator LoadLevelData()
//    {
        //opHandle = Addressables.LoadAssetAsync<LevelData>("Levels\\" + key);
        //yield return opHandle;
/*
        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            levelData = opHandle.Result;
        }
        else
        {
            Debug.LogError("oula");
        }*/
//    }

    void OnDestroy()
    {
        //Addressables.Release(opHandle);
    }
}
