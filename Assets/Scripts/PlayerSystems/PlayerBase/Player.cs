
using PlayerSystems.HandsSystem;
using PlayerSystems.MovementFSMCore;
using PlayerSystems.PlayerInput;
using UnityEngine;
using Player.Interaction;

namespace PlayerSystems.PlayerBase
{

    public class Player : MonoBehaviour
    {
        [SerializeField] PlayerInteraction m_PlayerInteraction;
        [SerializeField] InputManager m_InputManager;
        [SerializeField] HandsManager m_HandsManager;
        [SerializeField] Rigidbody m_PlayerRb;
        [SerializeField] Animator m_PlayerPrefabAnimator;
        [SerializeField] MovementFsmCore m_MovementFsmCore;

        public void Init()
        {
            m_PlayerInteraction.InitPlayerInteraction(m_HandsManager);
            m_PlayerInteraction.BindPerformInteraction(m_HandsManager.UseHand);
            m_InputManager.BindHandAction(m_PlayerInteraction.ActiveInteraction);
            m_InputManager.BindMergeHandInput(m_HandsManager.MergeFood);

            m_HandsManager.Init(m_PlayerRb, m_PlayerPrefabAnimator, m_MovementFsmCore.camera);
            InitFsmCore();
        }

        void Update()
        {
            m_MovementFsmCore.Update();
            m_PlayerInteraction.Update();
            UpdateFsmJumpHeld();
        }

        private void FixedUpdate()
        {
            m_MovementFsmCore.FixedUpdate();
        }

        public void InitUI(UI.UIManager uIManager)
        {
            //Stamina---------
            m_MovementFsmCore.BindStaminaRegeneration(uIManager.playerHUD.UpdateStaminaFill);

            //Inputs----------
            m_InputManager.BindTogglePauseMenu(uIManager.pauseMenu.ToggleActiveMenuState);

            //Interaction-----
            m_PlayerInteraction.BindOnInteractionUI(uIManager.playerHUD.playerInteractionUI.StartInteraction, uIManager.playerHUD.playerInteractionUI.SetActiveInteractionText);
        }

        private void UpdateFsmJumpHeld()
        {
            m_MovementFsmCore.jumpHeld = m_InputManager.GetJumpHeld();
        }

        public void InitFsmCore()
        {
            m_MovementFsmCore.Init(m_PlayerRb);
            m_InputManager.BindWasdMovement(m_MovementFsmCore.OnMovementInputEvent);
            m_InputManager.BindJump(m_MovementFsmCore.OnJumpInputEvent);
            m_InputManager.BindDash(m_MovementFsmCore.OnDashInputEvent);

            m_HandsManager.BindUpdateDashState(m_MovementFsmCore.UpdateDashState);
            m_HandsManager.BindUpdateDoubleJumpState(m_MovementFsmCore.UpdateDoubleJumpState);
            m_HandsManager.BindUpdateWallRunState(m_MovementFsmCore.UpdateWallRunState);
        }

        public void SetPosition(Vector3 newPos)
        {
            m_PlayerRb.position = newPos;
            m_PlayerRb.velocity = Vector3.zero;
        }

        public void ClearPlayerStamina()
        {
            m_MovementFsmCore.ClearStamina();
        }

        public void CrunchFoodInHands(bool forceAnim)
        {
            m_HandsManager.CrunchFoodInHands(forceAnim);
        }

        public float GetGroundedTime()
        {
            return m_MovementFsmCore.GetGroundedTime();
        }
    }
}