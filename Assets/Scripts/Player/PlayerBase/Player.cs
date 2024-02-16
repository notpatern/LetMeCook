using UnityEngine;

namespace Player
{

    public class Player : MonoBehaviour
    {
        [SerializeField] Interaction.PlayerInteraction playerInteraction;
        [SerializeField] HandSystem.HandsManager handsManager;
        [SerializeField] GameObject playerPrefab;

        void Start()
        {
            handsManager = new HandSystem.HandsManager();

            playerInteraction.BindPerformInteraction(handsManager.LoadHand);
        }

        void Update()
        {
            playerInteraction.Update(Time.deltaTime);
        }
    }
}