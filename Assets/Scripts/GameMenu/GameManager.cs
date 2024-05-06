using RecipeSystem;
using UnityEngine;

namespace Manager
{
    public class GameManager : LevelManager
    {
        [SerializeField] protected PlayerSystems.PlayerBase.Player m_Player;
        [SerializeField] protected GameEndCondition m_GameEndCondition;
        [SerializeField, Tooltip("Can be null")] protected LevelData m_NextLevelData;
        [SerializeField] GameEventScriptableObject m_LoadPlayerTransform;
        [SerializeField] RecipesManager m_RecipesManager;

        int m_Score = 0;
        int m_RecipesNb = 0;
        int m_CompletedRecipes = 0;

        protected bool m_IsEndStateInit = false;
        protected float m_LevelDuration = 0f;

        public static int maxDecalsNumber = 25;

        override protected void Awake()
        { 
            base.Awake();

            InitRecipeManager();

            m_Player.Init(m_RecipesManager);
            m_Player.InitUI(m_UiManager);

            m_LevelDuration = m_RecipesManager.GetLevelDurationBasedOnRecipesDataBase();

            InitEndCondition();
        }

        protected virtual void InitRecipeManager()
        {
            m_EndconditionParentUI.gameObject.SetActive(true);
            m_RecipesManager.Init(this, m_UiManager.recipeUI);
        }

        protected virtual void InitEndCondition()
        {
            if (m_IsEndStateInit) return;

            m_GameEndCondition = new DefaultGameEndCondition();
            m_GameEndCondition.InitGameEndCondition(m_LevelDuration, m_UiManager.endConditionUI);
            m_GameEndCondition.BindOnEndCondition(() =>
            {
                TriggerEndScreenSystem();
            });

            OnEndConditionInitialized();
        }

        protected void TriggerEndScreenSystem()
        {
            m_Player.gameObject.SetActive(false);
            m_MusicManager.IncreaseMusicTypeOffsetAmount();
            m_UiManager.pauseMenu.SetBlockPauseMenu(true, true);
            m_UiManager.endScreen.InitEndScreen(new TempScoreContainer(m_Score, m_RecipesNb, m_CompletedRecipes, m_LevelData.requiredScore, m_Player.GetGroundedTime()), m_NextLevelData, m_LevelData);
            m_UiManager.endScreen.SetActive(true);
        }

        protected void OnEndConditionInitialized()
        {
            m_IsEndStateInit = true;
        }

        protected virtual void Start()
        {
            m_LoadPlayerTransform.TriggerEvent(m_Player.transform);
        }

        protected override void Update()
        {
            base.Update();
            if (m_IsEndStateInit)
            {
                m_GameEndCondition.Update(Time.deltaTime);
            }
        }

        public void ForceEndConditionTimerValue(float value)
        {
            m_GameEndCondition.m_Timer = value;
        }

        public void AddScore(int scoreAmount)
        {
            m_Score += scoreAmount;

            m_UiManager.UpdateScore(m_Score);
        }

        public void AddRecipesCount(int amount)
        {
            m_RecipesNb += amount;
        }

        public void AddAcomplishedRecipes(int amount)
        {
            m_CompletedRecipes += amount;
        }
    }
}