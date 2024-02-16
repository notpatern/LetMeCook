using Player.Input;
using Player.Interaction;
using UnityEngine;

namespace Player
{

    public class Player : MonoBehaviour
    {
        [SerializeField] Interaction.PlayerInteraction playerInteraction;
        Input.InputManager inputManager;
        [SerializeField] HandSystem.HandsManager handsManager;
        [SerializeField] GameObject playerPrefab;

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
    }
}