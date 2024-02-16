using Player.HandSystem;
using UnityEngine;

namespace Player
{

    public class Player : MonoBehaviour
    {
        [SerializeField] Interaction.PlayerInteraction playerInteraction;
        [SerializeField] HandSystem.HandsManager handsManager;
        [SerializeField] GameObject playerPrefab;

        private void Start()
        {
            handsManager = new HandsManager();

            playerInteraction.BindPerformInteraction(handsManager.LoadHand);
        }
    }
}