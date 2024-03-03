using System;
using PlayerSystems.HandsSystem;
using PlayerSystems.MovementFSMCore;
using PlayerSystems.PlayerInput;
using UnityEngine;

namespace PlayerSystems.PlayerBase
{

    public class Player : MonoBehaviour
    {
        [SerializeField] global::Player.Interaction.PlayerInteraction playerInteraction;
        [SerializeField] InputManager inputManager;
        [SerializeField] HandsManager handsManager;
        public MovementFsmCore movementFsmCore;

        void Start()
        {
            movementFsmCore.Init();
            playerInteraction.BindPerformInteraction(handsManager.UseHand);
            inputManager.BindHandAction(playerInteraction.ActiveInteraction);
            
            InitFsmCore();
            handsManager.Init();
        }

        void Update()
        {
            UpdateFsmJumpHeldInput();
            movementFsmCore.Update();
            playerInteraction.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            movementFsmCore.FixedUpdate();
        }

        private void UpdateFsmJumpHeldInput()
        {
            movementFsmCore.jumpHeld = inputManager.GetJumpHeld();
        }

        public void InitUIEvent(UI.UIManager uIManager)
        {
            //Inputs----------
            inputManager.BindTogglePauseMenu(uIManager.pauseMenu.ToggleActiveMenuState);

            //Interaction-----
            playerInteraction.BindOnInteractionUI(uIManager.playerHUD.playerInteractionUI.StartInteraction, uIManager.playerHUD.playerInteractionUI.SetActiveInteractionText);
        }

        public void InitFsmCore()
        {
            inputManager.BindWasdMovement(movementFsmCore.OnMovementInputEvent);
            inputManager.BindJump(movementFsmCore.OnJumpInputEvent);
            inputManager.BindDash(movementFsmCore.OnDashInputEvent);
        }
    }
}