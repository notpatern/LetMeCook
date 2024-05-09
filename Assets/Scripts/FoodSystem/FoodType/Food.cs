using Player.Interaction;
using UnityEngine;
using System.Collections.Generic;
using ItemLaunch;
using Manager;

namespace FoodSystem.FoodType
{
    public abstract class Food : MonoBehaviour, IInteractable, IDestructible
    {
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

        protected virtual void Awake()
        {
            currentDecalProjector = decalProjector;
            currentFoodFog = foodFog;
        }

        public GameObject StartInteraction()
        {
            return gameObject;
        }

        private void FixedUpdate()
        {
            rb.drag = Grounded() ? 5 : 0;
        }

        public bool Grounded()
        {
            return Physics.Raycast(
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
        }

        public virtual void RemoveFromHand()
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            transform.SetParent(null);
            SetActiveColliders(true);
            ChangeLayer("Food");

            trailRenderer.enabled = true;
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
