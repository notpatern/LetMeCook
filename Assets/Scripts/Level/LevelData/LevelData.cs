using UnityEngine;
using Dialog;

[CreateAssetMenu(fileName = "LevelData", menuName = "LetMeCook/LevelData/LevelData", order = 9)]
public class LevelData : ScriptableObject
{
    public LevelUIData levelUIData;
    public DialogLevelData dialogLevelData;
}
