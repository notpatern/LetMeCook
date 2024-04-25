using RecipeSystem;
using UnityEngine;

namespace Manager
{
    public class GameManager : LevelManager
    {
        [SerializeField] PlayerSystems.PlayerBase.Player m_Player;
        [SerializeField] GameEndCondition m_GameEndCondition;
        [SerializeField, Tooltip("Can be null")] LevelData m_NextLevelData;
        [SerializeField] GameEventScriptableObject m_LoadPlayerTransform;
        [SerializeField] RecipesManager m_RecipesManager;

        int score = 0;
        int recipesNb = 0;
        int completedRecipes = 0;

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
                m_MusicManager.IncreaseMusicTypeOffsetAmount();
                m_UiManager.pauseMenu.SetBlockPauseMenu(true, true);
                m_UiManager.endScreen.InitEndScreen(new TempScoreContainer(score, recipesNb, completedRecipes, m_LevelData.requiredScore, m_Player.GetGroundedTime()), m_NextLevelData);
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

        public void AddScore(int scoreAmount)
        {
            score += scoreAmount;
        }

        public void AddRecipesCount(int amount)
        {
            recipesNb += amount;
        }

        public void AddAcomplishedRecipes(int amount)
        {
            completedRecipes += amount;
        }
    }
}