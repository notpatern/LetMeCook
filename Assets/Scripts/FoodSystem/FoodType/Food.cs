using Player.Interaction;
using UnityEngine;
using System.Collections.Generic;
using ItemLaunch;
using Manager;
using Audio;
using FMOD.Studio;
using FMODUnity;

namespace FoodSystem.FoodType
{
    public abstract class Food : MonoBehaviour, IInteractable, IDestructible
    {
        bool isInHand = false;
        bool isGrounded = false;
        [SerializeField] public Collider[] cols;
        [SerializeField] Rigidbody rb;
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] LayerMask isGround;
        [SerializeField] LaunchableItem launchableItem;
        [SerializeField] protected GameObject decalProjector;
        protected GameObject currentDecalProjector;
        [SerializeField] protected GameObject foodFog;
        protected GameObject currentFoodFog;
        [SerializeField] float groundedDistance;

        static GameObject[] decals = new GameObject[GameManager.maxDecalsNumber];
        static int currentFoodDecals = 0;

        EventInstance airSound;
        PLAYBACK_STATE pbState;

        protected virtual void Awake()
        {
            currentDecalProjector = decalProjector;
            currentFoodFog = foodFog;
        }

        public GameObject StartInteraction()
        {
            return gameObject;
        }

        void Update()
        {
            if (!isInHand)
            {
                CheckGrounded();

                airSound.getPlaybackState(out pbState);
                if(!isGrounded && pbState == PLAYBACK_STATE.STOPPED)
                {
                    airSound = AudioManager.s_Instance.CreateInstance(AudioManager.s_Instance.m_AudioSoundData.m_FoodAirThrowingEffect);
                    RuntimeManager.AttachInstanceToGameObject(airSound, transform);
                    airSound.start();
                }
                else if(isGrounded && pbState == PLAYBACK_STATE.PLAYING)
                {
                    airSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    airSound.release();
                }
            }
        }

        private void FixedUpdate()
        {
            if (!isInHand)
            {
                rb.drag = isGrounded ? 5 : 0;
            }
        }

        public void CheckGrounded()
        {
            isGrounded = Physics.Raycast(
                rb.position,
                Vector3.down,
                groundedDistance,
                isGround
            );
        }

        private void OnCollisionEnter(Collision collision)
        {
            int id = 0;
            if(currentFoodDecals >= GameManager.maxDecalsNumber)
            {
                currentFoodDecals = 0;
            }
            else
            {
                id = currentFoodDecals;
            }

            if(decals[currentFoodDecals])
            {
                Destroy(decals[currentFoodDecals]);
            }

            GameObject decal = Instantiate(currentDecalProjector, collision.contacts[0].point, Quaternion.LookRotation(-collision.contacts[0].normal));
            Vector3 rotation = new Vector3(decal.transform.rotation.eulerAngles.x, decal.transform.rotation.eulerAngles.y, Random.Range(0f, 360f));
            decal.transform.rotation = Quaternion.Euler(rotation);
            decal.transform.SetParent(collision.transform);

            decals[id] = decal;
            currentFoodDecals++;
            Instantiate(currentFoodFog, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));

            AudioManager.s_Instance.PlayOneShot(AudioManager.s_Instance.m_AudioSoundData.m_FoodBounceImpact, transform.position);
        }

        public abstract string GetContext();

        public abstract void AddFood(SimpleFood newFood);
        public abstract void AddFood(MergedFood mergedFood);
        public abstract List<FoodData> GetFoodDatas();

        public void SetActiveColliders(bool state)
        {
            foreach (var col in cols)
            {
                col.enabled = state;
            }
        }

        public void SetActiveTrails(bool state)
        {
            trailRenderer.enabled = state;
        }

        public virtual void PutInHand(Transform hand)
        {
            SetActiveColliders(false);
            launchableItem.QuitBezierCurve();
            transform.SetParent(hand);
            transform.position = hand.position;
            transform.localRotation = Quaternion.identity;
            ChangeLayer("Player");

            trailRenderer.enabled = false;
            rb.isKinematic = true;
            isInHand = true;
            airSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public virtual void RemoveFromHand()
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            transform.SetParent(null);
            SetActiveColliders(true);
            ChangeLayer("Food");

            trailRenderer.enabled = true;
            isInHand = false;
        }

        void ChangeLayer(string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);
            Transform[] children = GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (Transform child in children)
            {
                child.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }

        public void LaunchFood(Vector3 launchForce)
        {
            rb.AddForce(launchForce, ForceMode.Impulse);
        }
    }
}
