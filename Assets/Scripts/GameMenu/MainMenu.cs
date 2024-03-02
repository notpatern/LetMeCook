using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button levelSelectorBtn;

    void Start()
    {
        levelSelectorBtn.onClick.AddListener(() => {LevelLoader.s_instance.LoadLevel("levelselector");});
    }
}
