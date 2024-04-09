using UnityEngine;

namespace Manager
{
    public class GameManager : LevelManager
    {
        [SerializeField] PlayerSystems.PlayerBase.Player m_Player;
        [SerializeField] GameEndCondition m_GameEndCondition;
        [SerializeField, Tooltip("Can be null")] LevelData m_NextLevelData;
        [SerializeField] GameEventScriptableObject m_LoadPlayerTransform;

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
                m_UiManager.pauseMenu.SetBlockPauseMenu(true, true);
                m_UiManager.endScreen.InitEndScreen(new TempScoreContainer(score, recipesNb, completedRecipes, m_LevelData.requiredScore), m_NextLevelData);
                m_UiManager.endScreen.SetActive(true);
            });
        }

        void Start()
        {
            m_LoadPlayerTransform.TriggerEvent(m_Player.transform);
        }

        private void Update()
        {
            m_GameEndCondition.Update(Time.deltaTime);
        }
    }
}