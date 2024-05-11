using Player.Interaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace FoodSystem.FoodMachinery
{
    public class FoodDropper : MonoBehaviour, IInteractable
    {
        [FormerlySerializedAs("_data")] public FoodData data;
        [SerializeField] Animator animator;

        public virtual GameObject StartInteraction()
        {
            animator.SetTrigger("GetFood");
            GameObject food = Instantiate(data.prefab);
            food.GetComponent<Animator>().SetTrigger("FoodSpawn");
            return food;
        }

        public string GetContext() => data.foodName;
    }
}
