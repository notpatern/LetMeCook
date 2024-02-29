using UI;
using UnityEngine;

namespace Manager
{
    public class GameManager : LevelManager
    {
        [SerializeField] LevelData levelData;
        [SerializeField] Player.Player player;
        UIManager uiManager;

        override protected void Awake()
        { 
            base.Awake();
            uiManager = new UIManager();
            uiManager.LoadUI(levelData.levelUIData);
            player.InitUIEvent(uiManager);
            
            LevelScoreDataTransmetor levelScoreDataTransmetor = Instantiate(new GameObject().AddComponent<LevelScoreDataTransmetor>());
            DontDestroyOnLoad(levelScoreDataTransmetor);
        }

        void Start()
        {
            LevelLoader.s_instance.LoadLevel("EndGame");
        }
    }
}