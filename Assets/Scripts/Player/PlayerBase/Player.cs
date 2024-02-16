using UnityEngine;

namespace Player
{

    public class Player : MonoBehaviour
    {
        [SerializeField] Interaction.PlayerInteraction playerInteraction;
        [SerializeField] GameObject playerPrefab;

        private void Start()
        {
            playerInteraction.BindPerformInteraction(LoadHand);
        }

        void LoadHand(GameObject food, int id)
        {
            Debug.Log("J'équipe :" + food + " id : " + id);
        }
    }
}