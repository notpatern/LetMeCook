using UnityEngine;

[CreateAssetMenu(fileName ="LevelUIData", menuName ="LetMeCook/LevelData/LevelUIData")]
public class LevelUIData : ScriptableObject
{
    public GameObject canvasPrefab;
    public GameObject playerHUBPrefab;
    public GameObject pauseMenuPrefab;
    public GameObject endScreenMenuPrefab;

    [Header("Options Menu")]
    public bool isPauseMenuChild;
    public GameObject optionMenu;

    [Header("Dialog Menu")]
    public GameObject dialogMenu;

    [Header("GameMode")]
    public GameObject endConditionPrefab;
    public GameObject playerWarningText;
    public GameObject scorePrefab;

    [Header("Recipe UI")]
    public GameObject recipeContentParent;
}
