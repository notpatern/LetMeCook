using Player.Interaction;
using UnityEngine;
using System.Collections.Generic;
using ItemLaunch;

namespace FoodSystem.FoodType
{
    public abstract class Food : MonoBehaviour, IInteractable, IDestructible
    {
        [SerializeField] public Collider[] cols;
        [SerializeField] Rigidbody rb;
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] LayerMask isGround;
        [SerializeField] LaunchableItem launchableItem;

        public GameObject StartInteraction()
        {
            return gameObject;
        }

        private void FixedUpdate()
        {
            rb.drag = Grounded() ? 10 : 0;
        }

        public bool Grounded()
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

        public void SetActiveColliders(bool state)
        {
            foreach (var col in cols)
            {
                col.enabled = state;
            }
        }

        public virtual void PutInHand(Transform hand)
        {
            SetActiveColliders(false);
            launchableItem.QuitBezierCurve();
            rb.isKinematic = true;
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
