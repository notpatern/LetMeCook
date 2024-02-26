using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{

    public class Player : MonoBehaviour
    {
        [SerializeField] Interaction.PlayerInteraction playerInteraction;
        Input.InputManager inputManager;
        [SerializeField] HandSystem.HandsManager handsManager;

        void Start()
        {
            handsManager = new HandSystem.HandsManager();

            inputManager = GetComponent<Input.InputManager>();

            playerInteraction.Init();

            playerInteraction.BindPerformInteraction(handsManager.LoadHand);
            inputManager.BindHandAction(playerInteraction.ActiveInteraction);

        }

        void Update()
        {
            playerInteraction.Update(Time.deltaTime);
        }

        public void InitUIEvent(UIManager uIManager)
        {
            //inputManager.BindTogglePauseMenu(uIManager.pauseMenu.ToggleMenuState);
        }
    }
}