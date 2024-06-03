using UnityEngine;
using Dialog;

[CreateAssetMenu(fileName = "LevelData", menuName = "LetMeCook/LevelData/LevelData", order = 9)]
public class LevelData : ScriptableObject
{
    public Sprite mapPreviewIcon; 
    public string levelDescription;
    public LevelUIData levelUIData;
    public DialogLevelData dialogLevelData;
    public LevelMusicData levelMusicData;
    public string linkedScenePath;
    public int levelID = 0;
    public int requiredScore = 100;
}
