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

            m_GameEndCondition = new DefaultGameEndCondition();
            m_GameEndCondition.InitGameEndCondition(m_LevelData.levelDuration);
            m_GameEndCondition.BindOnEndCondition(() =>
            {
                m_UiManager.endScreen.InitEndScreen();
                m_UiManager.endScreen.SetActive(true);
            });
        }

        private void Update()
        {
            m_GameEndCondition.Update(Time.deltaTime);
            m_UiManager.endConditionUI.UpdateText((m_GameEndCondition.m_Timer/60f).ToString("00.00") + "s");
        }
    }
}