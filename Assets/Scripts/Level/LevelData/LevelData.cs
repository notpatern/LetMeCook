using UnityEngine;
using Dialog;

[CreateAssetMenu(fileName = "LevelData", menuName = "LetMeCook/LevelData/LevelData", order = 9)]
public class LevelData : ScriptableObject
{
    public LevelUIData levelUIData;
    public DialogLevelData dialogLevelData;
    public string linkedScenePath;
    public int levelID = 0;
    public float levelDuration = 120f;
    public int requiredScore = 100;
}
