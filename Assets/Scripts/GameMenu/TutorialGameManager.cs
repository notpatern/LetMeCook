using UnityEngine;

namespace Manager
{
    public class TutorialGameManager : GameManager
    {
        [SerializeField] Transform respawnTr;

        void OnTriggerEnter(Collider other)
        {
            if (!isEndStateInit)
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
            m_GameEndCondition = new DefaultGameEndCondition();
            m_GameEndCondition.InitGameEndCondition(m_LevelData.levelDuration, m_UiManager.endConditionUI);
            m_GameEndCondition.BindOnEndCondition(() =>
            {
                m_EndconditionParentUI.gameObject.SetActive(false);
                isEndStateInit = false;
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