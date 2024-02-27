using UnityEngine;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] DefaultLevelData defaultLevelData;
        protected virtual void Awake()
        {
            if(!LevelLoader.instance)
            {
                Instantiate(defaultLevelData.LevelLoader);
            }
        }
    }
}