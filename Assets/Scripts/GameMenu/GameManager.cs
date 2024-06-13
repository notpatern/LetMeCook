 using Audio;
using RecipeSystem;
using RecipeSystem.Core;
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
        [SerializeField] GameObject scoreWorldInfoUIPrefab;

        protected int m_Score = 0;
        protected int m_RecipesNb = 0;
        protected int m_CompletedRecipes = 0;
        protected int m_BonusRecipes = 0;

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

            UpdateScoreUI();
        }

        protected virtual void InitEndCondition()
        {
            if (m_IsEndStateInit) return;

            m_GameEndCondition = new DefaultGameEndCondition();
            m_GameEndCondition.InitGameEndCondition(m_LevelDuration, m_UiManager.endConditionUI, m_EndconditionHandParentUI);
            m_GameEndCondition.BindOnEndCondition(() =>
            {
                TriggerEndScreenSystem();
            });

            OnEndConditionInitialized();
        }

        protected void TriggerEndScreenSystem()
        {
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_EndClockSound);
            m_Player.gameObject.SetActive(false);
            m_Player.OnEndLevelTriggered();
            m_MusicManager.IncreaseMusicTypeOffsetAmount();
            m_UiManager.pauseMenu.SetBlockPauseMenu(true, true);
            m_UiManager.endScreen.InitEndScreen(new TempScoreContainer(m_Score, m_RecipesNb, m_CompletedRecipes, m_BonusRecipes, m_LevelData.requiredScore, m_Player.GetGroundedTime()), m_NextLevelData, m_LevelData);
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

            if(m_Score < 0)
            {
                m_Score = 0;
            }

            UpdateScoreUI();
        }

        public void AddScore(int scoreAmount, Vector3 foodPositionForFeedbackWorldUIText, int timeAmount = 0)
        {
            Quaternion lookAtPlayer = Quaternion.LookRotation(foodPositionForFeedbackWorldUIText - m_Player.transform.position, Vector3.up);
            Instantiate(scoreWorldInfoUIPrefab, foodPositionForFeedbackWorldUIText, lookAtPlayer).GetComponent<ScoreWorldInfoUI>().InitText(scoreAmount, timeAmount);

            AddScore(scoreAmount + timeAmount);
        }

        void UpdateScoreUI()
        {
            int maxMainRecipesFeedScore = m_RecipesManager.GetMaxScoreOnMainRecipesFeed();

            int scoreWithoutBonus = 0;

            if (m_RecipesManager.dataBase.randomFillerRecipes != null && m_RecipesManager.dataBase.randomFillerRecipes.Length > 0)
            {
                scoreWithoutBonus = m_Score - m_BonusRecipes * m_RecipesManager.dataBase.randomFillerRecipes[0].addedScore;
            }
            else
            {
                Debug.LogError("GameManager Score, no randomFillerRecipes is null or length == 0");
            }

            int requiredScoreUntilNextStar = m_LevelData.requiredScore > m_Score ? m_LevelData.requiredScore - m_Score : m_UiManager.endScreen.GetRequiredScoreUntilNextStar(scoreWithoutBonus, maxMainRecipesFeedScore);

            m_UiManager.UpdateScore(m_Score, m_UiManager.endScreen.GetUnlockedStars(scoreWithoutBonus, maxMainRecipesFeedScore, m_LevelData.requiredScore, m_Score >= m_LevelData.requiredScore), requiredScoreUntilNextStar);
        }

        public void AddRecipesCount(int amount)
        {
            m_RecipesNb += amount;
        }

        public void AddAcomplishedRecipes(GameRecipe gameRecipe)
        {
            if (gameRecipe.isBonusRecipe)
            {
                m_BonusRecipes++;
            }
            else
            {
                m_CompletedRecipes++;
            }
        }
    }
}