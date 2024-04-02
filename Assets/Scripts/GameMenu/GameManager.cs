using UnityEngine;

namespace Manager
{
    public class GameManager : LevelManager
    {
        [SerializeField] PlayerSystems.PlayerBase.Player m_Player;
        [SerializeField] GameEndCondition m_GameEndCondition;

        override protected void Awake()
        { 
            base.Awake();
            m_Player.InitUIEvent(m_UiManager);
            
            LevelScoreDataTransmetor levelScoreDataTransmetor = Instantiate(new GameObject().AddComponent<LevelScoreDataTransmetor>());
            DontDestroyOnLoad(levelScoreDataTransmetor.gameObject);

            m_GameEndCondition = new DefaultGameEndCondition();
            m_GameEndCondition.InitGameEndCondition(m_LevelData.levelDuration);
            m_GameEndCondition.BindOnEndCondition(() =>
            {
                LevelLoader.s_instance.LoadLevel(1);//endScene buildIndex
            });
        }

        private void Update()
        {
            m_GameEndCondition.Update(Time.deltaTime);
            m_UiManager.endConditionUI.UpdateText((m_GameEndCondition.m_Timer/60f).ToString("00.00") + "s");
        }
    }
}