using UnityEngine;

namespace Manager
{
    public class GameManager : LevelManager
    {
        [SerializeField] PlayerSystems.PlayerBase.Player m_Player;


        override protected void Awake()
        { 
            base.Awake();
            m_Player.InitUIEvent(m_UiManager);
            
            LevelScoreDataTransmetor levelScoreDataTransmetor = Instantiate(new GameObject().AddComponent<LevelScoreDataTransmetor>());
            DontDestroyOnLoad(levelScoreDataTransmetor.gameObject);
        }
    }
}