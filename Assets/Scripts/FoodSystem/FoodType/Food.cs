using Player.Interaction;
using UnityEngine;
using System.Collections.Generic;

namespace FoodSystem.FoodType
{
    public abstract class Food : MonoBehaviour, IInteractable, IDestructible
    {
        Collider[] col;
        [SerializeField] Rigidbody rb;
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] LayerMask isGround;

        void Awake()
        {
            col = GetComponents<Collider>();
        }

        public GameObject StartInteraction()
        {
            return gameObject;
        }

        private void FixedUpdate()
        {
            rb.drag = Grounded() ? 10 : 0;
        }

        private bool Grounded()
        {
            return Physics.Raycast(
                rb.position,
                Vector3.down,
                .5f,
                isGround
            );
        }

        public abstract string GetContext();

        public abstract void AddFood(SimpleFood newFood);
        public abstract void AddFood(MergedFood mergedFood);
        public abstract List<FoodData> GetFoodDatas();

        public virtual void PutInHand(Transform hand)
        {
            rb.isKinematic = true;
            foreach (Collider coll in col)
            {
                coll.enabled = false;
            }
            transform.SetParent(hand);
            transform.position = hand.position;
            transform.localRotation = Quaternion.identity;
            ChangeLayer("Player");

            trailRenderer.enabled = false;
        }

        public virtual void RemoveFromHand()
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            transform.SetParent(null);
            foreach (Collider coll in col)
            {
                coll.enabled = true;
            }
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
