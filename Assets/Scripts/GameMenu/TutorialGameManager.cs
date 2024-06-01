using UnityEngine;

namespace Manager
{
    public class TutorialGameManager : GameManager
    {
        [SerializeField] Transform respawnTr;
        protected override void Awake()
        {
            base.Awake();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!m_IsEndStateInit)
            {
                base.InitRecipeManager();
                StartInitDefaultEndCondition();
            }
        }

        protected override void InitEndCondition()
        {
            
        }

        protected override void InitRecipeManager()
        {
            
        }


        void StartInitDefaultEndCondition()
        {
            m_Score = 0;
            m_RecipesNb = 0;
            m_CompletedRecipes = 0;
            m_BonusRecipes = 0;
            m_GameEndCondition = new DefaultGameEndCondition();
            m_GameEndCondition.InitGameEndCondition(m_LevelDuration, m_UiManager.endConditionUI);
            m_GameEndCondition.BindOnEndCondition(() =>
            {
                m_EndconditionParentUI.gameObject.SetActive(false);
                m_IsEndStateInit = false;
                m_Player.SetPosition(respawnTr.position);
                m_Player.CrunchFoodInHands(false);
            });

            OnEndConditionInitialized();
        }

        public void TriggerFinishLevelEndCondition()
        {
            TriggerEndScreenSystem();
        }
    }

}