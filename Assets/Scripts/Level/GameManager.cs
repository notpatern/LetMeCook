using UI;
using UnityEngine;

namespace Manager
{
    public class GameManager : LevelManager
    {
        [SerializeField] LevelData levelData;
        [SerializeField] Player.Player player;
        UIManager uiManager;
        void Awake()
        { 
            uiManager = new UIManager();
            uiManager.LoadUI(levelData.levelUIData);
            player.InitUIEvent(uiManager);
        }
    }
}