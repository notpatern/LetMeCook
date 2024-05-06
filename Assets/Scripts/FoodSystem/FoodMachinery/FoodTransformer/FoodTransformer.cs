using ItemLaunch;
using ParticleSystemUtility;
using UnityEngine;
using UnityEngine.UI;
using FoodSystem.FoodType;
using TMPro;
using Unity.VisualScripting;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    [RequireComponent(typeof(ItemLauncher))]
    public abstract class FoodTransformer : FoodCollector
    {
        [Header("Actions with movetech and food")]
        [SerializeField] GameEventScriptableObject loadPlayerTransformAtStart;
        [SerializeField] GameObject vines;
        [SerializeField] GameEventScriptableObject simpleNotCookedFoodChangeMoveTechStateEvent;
        [Header("World UI")]
        [SerializeField] GameObject progressBarUI;
        [SerializeField] Image progressFill;
        Transform playerTr;
        [SerializeField] Animator animator;
        [SerializeField] ParticleInstanceManager cookParticleInstanceManager;
        [SerializeField] float curveFadeTime;

        protected ItemLauncher launcher;

        [Header("Food during cooking")]
        [SerializeField] protected Transform cookingFoodPos;

        float _timer = 0f;
        [SerializeField] protected float cookingTime = 5f;
        protected bool _cooking = false;

        void Awake() 
        {
            loadPlayerTransformAtStart.BindEventAction(LoadPlayerTransform);
            simpleNotCookedFoodChangeMoveTechStateEvent.BindEventAction(SimpleFoodChangeMoveTechState);
            vines.SetActive(false);
            progressBarUI.SetActive(false);
            launcher = GetComponent<ItemLauncher>();
        }

        protected virtual void Start()
        {
            launcher.curveFadeTime = curveFadeTime;
            cookParticleInstanceManager.Stop(false);
        }

        void LoadPlayerTransform(object args)
        {
            playerTr = (Transform)args;
        }

        void SimpleFoodChangeMoveTechState(object isActive)
        {
            vines.SetActive((bool)isActive);
        }

        protected Vector3 PerformVectorLerp(Vector3 from, Vector3 to, float t)
        {
            return from + (to - from) * Mathf.Clamp01(t);
        }
        protected override void OnFoodCollected()
        {
            if (!CheckIfCanCook(collectedFoodData[0]))
            {
                ResetCollector();
                return;
            }

            launcher.ChangeState(true);

            cookParticleInstanceManager.Play();
            animator.SetTrigger("Cook");

            progressBarUI.SetActive(true);
            canCollect = false;
            _timer = 0f;
            _cooking = true;
            collectedFoodGo.transform.localPosition = Vector3.zero;
            collectedFoodGo.GetComponent<Food>().SetActiveColliders(false);
            collectedFoodGo.GetComponent<Food>().SetActiveTrails(false);
            collectedFoodGo.transform.SetParent(cookingFoodPos, false);
            collectedFoodGo.GetComponent<Collider>().enabled = false;
            collectedFoodGo.GetComponent<Rigidbody>().isKinematic = true;
        }

        protected virtual void Update()
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
            Destroy(collectedFoodGo);

            launcher.ChangeState(false);

            cookParticleInstanceManager.Stop(false);
            animator.SetTrigger("Throw");
            collectedFoodGo.GetComponent<Food>().SetActiveColliders(true);
            collectedFoodGo.GetComponent<Food>().SetActiveTrails(true);
            progressBarUI.SetActive(false);
            ResetCollector();
            canCollect = true;
            _cooking = false;
        }

        protected abstract bool CheckIfCanCook(FoodData foodData);
    }
}
