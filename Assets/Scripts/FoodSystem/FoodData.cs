using UnityEngine;

namespace FoodSystem
{
    [CreateAssetMenu(menuName = "LetMeCook/Food Data")]
    public class FoodData : ScriptableObject
    {
        [Header("General")]
        public string foodName;
        public GameObject prefab;

        [Header("Bonus skill")]
        public bool dash = false;
        public bool doubleJump = false;
        public bool wallRide = false;

        [Header("Next step")]
        public FoodData bakedFood;
        public FoodData choppedFood;
        public FoodData purifiedFood;
    }
}
