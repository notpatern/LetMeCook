using Player.Interaction;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace FoodSystem.FoodType
{
    public abstract class Food : MonoBehaviour, IInteractable
    {
        [SerializeField] BoxCollider col;
        [SerializeField] Rigidbody rb;
        [SerializeField] TrailRenderer trailRenderer;

        public GameObject StartInteraction()
        {
            return gameObject;
        }

        public abstract string GetContext();

        public abstract void AddFood(SimpleFood newFood);
        public abstract void AddFood(MergedFood mergedFood);
        public abstract List<FoodData> GetFoodDatas();

        public virtual void PutInHand(Transform hand)
        {
            rb.isKinematic = true;
            col.enabled = false;
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
            col.enabled = true;
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
