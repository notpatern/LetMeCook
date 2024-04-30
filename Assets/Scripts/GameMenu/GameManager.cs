using RecipeSystem;
using UI;
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

        protected int score = 0;
        protected int recipesNb = 0;
        protected int completedRecipes = 0;

        protected bool isEndStateInit = false;

        override protected void Awake()
        { 
            base.Awake();
            m_Player.Init();
            m_Player.InitUI(m_UiManager);

            InitRecipeManager();

            InitEndCondition();
        }

        protected virtual void InitRecipeManager()
        {
            m_EndconditionParentUI.gameObject.SetActive(true);
            m_RecipesManager.Init(this, m_UiManager.recipeUI);
        }

        protected virtual void InitEndCondition()
        {
            if (isEndStateInit) return;

            m_GameEndCondition = new DefaultGameEndCondition();
            m_GameEndCondition.InitGameEndCondition(m_LevelData.levelDuration, m_UiManager.endConditionUI);
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
            m_UiManager.endScreen.InitEndScreen(new TempScoreContainer(score, recipesNb, completedRecipes, m_LevelData.requiredScore, m_Player.GetGroundedTime()), m_NextLevelData);
            m_UiManager.endScreen.SetActive(true);
        }

        protected void OnEndConditionInitialized()
        {
            isEndStateInit = true;
        }

        void Start()
        {
            m_LoadPlayerTransform.TriggerEvent(m_Player.transform);
        }

        protected virtual void Update()
        {
            if (isEndStateInit)
            {
                m_GameEndCondition.Update(Time.deltaTime);
            }
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