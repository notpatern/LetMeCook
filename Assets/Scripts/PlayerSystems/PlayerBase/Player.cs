using UnityEngine;

namespace Player
{

    public class Player : MonoBehaviour
    {
        [SerializeField] Interaction.PlayerInteraction playerInteraction;
        [SerializeField] Input.InputManager inputManager;
        [SerializeField] HandSystem.HandsManager handsManager;

        void Start()
        {
            handsManager = new HandSystem.HandsManager();

            playerInteraction.BindPerformInteraction(handsManager.LoadHand);
            inputManager.BindHandAction(playerInteraction.ActiveInteraction);

        }

        void Update()
        {
            playerInteraction.Update(Time.deltaTime);
        }

        public void InitUIEvent(UI.UIManager uIManager)
        {
            //Inputs----------
            inputManager.BindTogglePauseMenu(uIManager.pauseMenu.ToggleActiveMenuState);

            //Interaction-----
            playerInteraction.BindOnInteractionUI(uIManager.playerHUD.playerInteractionUI.StartInteraction, uIManager.playerHUD.playerInteractionUI.SetActiveInteractionText);
        }
    }
}