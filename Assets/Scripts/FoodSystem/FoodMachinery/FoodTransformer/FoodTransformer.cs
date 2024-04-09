using ItemLaunch;
using UnityEngine;
using UnityEngine.UI;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    [RequireComponent(typeof(ItemLauncher))]
    public abstract class FoodTransformer : FoodCollector
    {
        [SerializeField] GameEventScriptableObject loadPlayerTransformAtStart;
        [Header("World UI")]
        [SerializeField] GameObject progressBarUI;
        [SerializeField] Image progressFill;
        Transform playerTr;

        protected ItemLauncher launcher;

        float _timer = 0f;
        [SerializeField] float cookingTime = 5f;
        bool _cooking = false;
        void Awake() 
        {
            loadPlayerTransformAtStart.BindEventAction(LoadPlayerTransform);
            progressBarUI.SetActive(false);
            launcher = GetComponent<ItemLauncher>(); 
        }

        void LoadPlayerTransform(object args)
        {
            playerTr = (Transform)args;
        }


        protected override void OnFoodCollected()
        {
            if (!CheckIfCanCook(collectedFoodData[0]))
            {
                ResetCollector();
                return;
            }

            progressBarUI.SetActive(true);
            canCollect = false;
            _timer = 0f;
            _cooking = true;
            Destroy(collectedFoodGo);
        }

        void Update()
        {
            if (!_cooking)
                return;

            if (_timer <= cookingTime)
            {
                progressBarUI.transform.LookAt(playerTr.position);

                progressFill.fillAmount = _timer / cookingTime;

                _timer += Time.deltaTime;
            }
            else
                ReleaseFood();
        }

        protected virtual void ReleaseFood()
        {
            progressBarUI.SetActive(false);
            ResetCollector();
            canCollect = true;
            _cooking = false;
        }

        protected abstract bool CheckIfCanCook(FoodData foodData);
    }
}
