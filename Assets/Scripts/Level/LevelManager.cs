using Player.Input;
using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] DefaultLevelData defaultLevelData;
        protected virtual void Start()
        {
            if(!LevelLoader.instance)
            {
                Instantiate(defaultLevelData.LevelLoader);
            }

            ControlsRemapping.LoadMap();
        }
    }
}