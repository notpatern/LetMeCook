using ItemLaunch;
using UnityEngine;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    [RequireComponent(typeof(ItemLauncher))]
    public abstract class FoodTransformer : FoodCollector
    {
        protected ItemLauncher launcher;

        float _timer = 0f;
        [SerializeField] float cookingTime = 5f;
        bool _cooking = false;

        void Awake() { launcher = GetComponent<ItemLauncher>(); }

        protected override void OnFoodCollected()
        {
            if (!CheckIfCanCook(collectedFoodData[0]))
            {
                ResetCollector();
                return;
            }
            
            canCollect = false;
            _timer = cookingTime;
            _cooking = true;
            Destroy(collectedFoodGo);
        }

        void Update()
        {
            if (!_cooking)
                return;
        
            if (_timer > 0f)
                _timer -= Time.deltaTime;
            else
                ReleaseFood();
        }

        protected virtual void ReleaseFood()
        {
            ResetCollector();
            canCollect = true;
            _cooking = false;
        }

        protected abstract bool CheckIfCanCook(FoodData foodData);
    }
}
