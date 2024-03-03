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
        [SerializeField] Rigidbody playerRb;
        public MovementFsmCore movementFsmCore;

        void Start()
        {
            playerInteraction.BindPerformInteraction(handsManager.UseHand);
            inputManager.BindHandAction(playerInteraction.ActiveInteraction);
            inputManager.BindMergeHandInput(handsManager.MergeFood);

            InitFsmCore();
            handsManager.Init(playerRb);
        }

        void Update()
        {
            movementFsmCore.Update();
            playerInteraction.Update(Time.deltaTime);
            UpdateFsmJumpHeld();
        }

        private void FixedUpdate()
        {
            movementFsmCore.FixedUpdate();
        }

        public void InitUIEvent(UI.UIManager uIManager)
        {
            //Inputs----------
            inputManager.BindTogglePauseMenu(uIManager.pauseMenu.ToggleActiveMenuState);

            //Interaction-----
            playerInteraction.BindOnInteractionUI(uIManager.playerHUD.playerInteractionUI.StartInteraction, uIManager.playerHUD.playerInteractionUI.SetActiveInteractionText);
        }

        private void UpdateFsmJumpHeld()
        {
            movementFsmCore.jumpHeld = inputManager.GetJumpHeld();
        }

        public void InitFsmCore()
        {
            movementFsmCore.Init(playerRb);
            inputManager.BindWasdMovement(movementFsmCore.OnMovementInputEvent);
            inputManager.BindJump(movementFsmCore.OnJumpInputEvent);
            inputManager.BindDash(movementFsmCore.OnDashInputEvent);
        }
    }
}