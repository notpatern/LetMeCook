using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.HandSystem
{
    [Serializable]
    public class Hands
    {
        [SerializeField] HandsType handsType;
        [SerializeField] private Transform foodPosition;
        [SerializeField] private Transform throwPoint;
        [SerializeField] private float throwForce;
        private GameObject handledFood;
        public bool isFoodHandle { get; private set; } = false;

        public void PutItHand(GameObject food)
        {
            //Add a function in food to put and remove in hand
            food.GetComponent<Rigidbody>().velocity = Vector3.zero;
            food.GetComponent<Rigidbody>().isKinematic = true;
            food.GetComponent<BoxCollider>().enabled = false;
            food.transform.position = foodPosition.position;
            food.transform.rotation = Quaternion.identity;
            food.transform.SetParent(foodPosition);
            handledFood = food;
            isFoodHandle = true;
        }

        public void ReleaseFood()
        {
            handledFood.GetComponent<Rigidbody>().isKinematic = false;
            handledFood.transform.SetParent(null);
            handledFood.transform.position = throwPoint.position;
            handledFood.GetComponent<BoxCollider>().enabled = true;
            handledFood.GetComponent<Rigidbody>().AddForce(throwPoint.forward * throwForce);
            handledFood = null;
            isFoodHandle = false;
        }
    }

    public enum HandsType
    {
        NONE = 0,
        LEFT = 1, RIGHT = 2
    }
}