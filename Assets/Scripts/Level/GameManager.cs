using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
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
