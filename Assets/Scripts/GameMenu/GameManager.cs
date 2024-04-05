using PlayerSystems.PlayerBase;
using UnityEngine;

namespace Manager
{
    public class GameManager : LevelManager
    {
        [SerializeField] PlayerSystems.PlayerBase.Player m_Player;
        [SerializeField] GameEndCondition m_GameEndCondition;

        //TEMP SCORE
        [SerializeField] int score = 0;
        int recipesNb = 10;
        int completedRecipes = 3;

        override protected void Awake()
        { 
            base.Awake();
            m_Player.Init();
            m_Player.InitUI(m_UiManager);

            m_GameEndCondition = new DefaultGameEndCondition();
            m_GameEndCondition.InitGameEndCondition(m_LevelData.levelDuration, m_UiManager.endConditionUI);
            m_GameEndCondition.BindOnEndCondition(() =>
            {
                m_Player.gameObject.SetActive(false);
                m_UiManager.endScreen.InitEndScreen(new TempScoreContainer(score, recipesNb, completedRecipes, m_LevelData.requiredScore));
                m_UiManager.endScreen.SetActive(true);
            });
        }

        private void Update()
        {
            m_GameEndCondition.Update(Time.deltaTime);
        }
    }
}