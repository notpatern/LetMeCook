using Player.Interaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace FoodSystem.FoodMachinery
{
    public class FoodDropper : MonoBehaviour, IInteractable
    {
        [FormerlySerializedAs("_data")] public FoodData data;

        public GameObject StartInteraction()
        {
            GameObject food = Instantiate(data.prefab);
            return food;
        }

        public string GetContext() => "dropper (" + data.foodName + ")";
    }
}
