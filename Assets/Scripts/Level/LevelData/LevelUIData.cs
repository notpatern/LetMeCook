using UnityEngine;

[CreateAssetMenu(fileName ="LevelUIData", menuName ="LetMeCook/LevelData/LevelUIData")]
public class LevelUIData : ScriptableObject
{
    public GameObject canvasPrefab;
    public GameObject playerHUBPrefab;
    public GameObject pauseMenuPrefab;

    [Header("Options Menu")]
    public bool isPauseMenuChild;
    public GameObject optionMenu;

    [Header("Dialog Menu")]
    public GameObject dialogMenu;

    [Header("GameMode")]
    public GameObject endConditionPrefab;
}
