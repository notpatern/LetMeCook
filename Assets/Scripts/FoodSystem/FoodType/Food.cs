using Player.Interaction;
using UnityEngine;
using System.Collections.Generic;

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
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            col.enabled = false;
            transform.SetParent(hand);
            transform.position = hand.position;
            transform.localRotation = Quaternion.identity;


            trailRenderer.enabled = false;
        }

        public virtual void RemoveFromHand()
        {
            rb.isKinematic = false;
            transform.SetParent(null);
            col.enabled = true;


            trailRenderer.enabled = true;
        }

        public void LaunchFood(Vector3 launchForce)
        {
            rb.AddForce(launchForce, ForceMode.Impulse);
        }
    }
}
