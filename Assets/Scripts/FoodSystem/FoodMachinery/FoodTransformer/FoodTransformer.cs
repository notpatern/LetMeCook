using ItemLaunch;
using ParticleSystemUtility;
using UnityEngine;
using UnityEngine.UI;
using FoodSystem.FoodType;
using System.Collections.Generic;
using Audio;
using FMOD.Studio;
using FMODUnity;

namespace FoodSystem.FoodMachinery.FoodTransformer
{
    [RequireComponent(typeof(ItemLauncher))]
    public abstract class FoodTransformer : FoodCollector
    {
        [Header("Actions with movetech and food")]
        [SerializeField] GameEventScriptableObject loadPlayerTransformAtStart;
        [SerializeField] GameObject vines;
        [SerializeField] GameObject deactivedVines;
        [SerializeField] GameEventScriptableObject simpleNotCookedFoodChangeMoveTechStateEvent;
        [Header("World UI")]
        [SerializeField] GameObject progressBarUI;
        [SerializeField] Image progressFill;
        Transform playerTr;
        [SerializeField] Animator animator;
        [SerializeField] ParticleInstanceManager cookParticleInstanceManager;
        [SerializeField] float curveFadeTime;

        List<SimpleFood> foodInQueue = new List<SimpleFood>();

        protected ItemLauncher launcher;
        [SerializeField] QueueTextFollow queueText;

        [Header("Food during cooking")]
        [SerializeField] protected Transform cookingFoodPos;

        float _timer = 0f;
        [SerializeField] protected float cookingTime = 5f;
        protected bool _cooking = false;

        [SerializeField] GameEventScriptableObject m_OnFoodPickupGameEvent;
        [SerializeField] GameObject m_QueueParticles;

        protected EventInstance cookingSound;
        PLAYBACK_STATE cookingPBState;

        protected virtual void Awake() 
        {
            loadPlayerTransformAtStart.BindEventAction(LoadPlayerTransform);
            simpleNotCookedFoodChangeMoveTechStateEvent.BindEventAction(SimpleFoodChangeMoveTechState);
            vines.SetActive(false);
            deactivedVines.SetActive(true);
            progressBarUI.SetActive(false);
            launcher = GetComponent<ItemLauncher>();
            m_OnFoodPickupGameEvent.BindEventAction(OnFoodPickupGameEventRemoveFromQueue);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            SimpleFood food = other.gameObject.GetComponentInParent<SimpleFood>();
            if(IsQueueFittable(food, true))
            {
                Instantiate(m_QueueParticles, other.transform.position, Quaternion.identity);
                foodInQueue.Add(food);
                UpdateQueueText();

                AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_FoodInCookingRange);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            SimpleFood food = other.gameObject.GetComponentInParent<SimpleFood>();

            RemoveFromQueue(food);
        }

        void OnFoodPickupGameEventRemoveFromQueue(object arg)
        {
            GameObject foodGo = (GameObject)arg;
            RemoveFromQueue(foodGo.GetComponent<SimpleFood>());

        }

        protected void CheckForPlayingCookingSound(EventReference eventReference)
        {
            cookingSound.getPlaybackState(out cookingPBState);
            if (cookingPBState != PLAYBACK_STATE.STOPPED)
            {
                cookingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                cookingSound.release();
            }

            if (_cooking)
            {
                cookingSound = AudioManager.s_Instance.Create3DInstance(eventReference, transform.position);
                cookingSound.start();
            }
        }

        protected void RemoveFromQueue(SimpleFood food)
        {
            if (IsQueueFittable(food, false))
            {
                foodInQueue.Remove(food);
                UpdateQueueText();
            }
        }

        void UpdateQueueText()
        {
            queueText.text.text = foodInQueue.Count + " Queued";
        }

        bool IsQueueFittable(SimpleFood food, bool isIn)
        {
            if (food != null && CheckIfCanCook(food.GetFoodDatas()[0]))
            {
                if ((!foodInQueue.Contains(food)) == isIn)
                {
                    return true;
                }
            }

            return false;
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
            deactivedVines.SetActive(!(bool)isActive);
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

            RemoveFromQueue(collectedFoodGo.GetComponent<SimpleFood>());

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
            collectedFoodGo.GetComponent<Food>().SetActiveTrails(true);
            collectedFoodGo.GetComponent<Food>().SetActiveColliders(true);
            progressBarUI.SetActive(false);
            ResetCollector();
            canCollect = true;
            _cooking = false;
        }

        protected abstract bool CheckIfCanCook(FoodData foodData);
    }
}
