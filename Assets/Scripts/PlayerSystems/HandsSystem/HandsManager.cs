using System;
using Player.HandSystem;
using UnityEngine;

namespace PlayerSystems.HandsSystem
{
    [Serializable]
    public class HandsManager
    {
        
        [SerializeField] private float throwForce;
        [SerializeField] private Hands leftHand;
        [SerializeField] private Hands rightHand;

        public void Init()
        {
            leftHand.InitData(throwForce);
            rightHand.InitData(throwForce);
        }
        
        public void UseHand(GameObject food, HandsType handsType)
        {
            switch (handsType)
            {
                case HandsType.NONE:
                    Debug.LogError("This should not happen but does.");
                    break;
                case HandsType.LEFT:
                    PerformHandAction(food, leftHand);
                    break;
                case HandsType.RIGHT:
                    PerformHandAction(food, rightHand);
                    break;
            }
        }

        public void PerformHandAction(GameObject food, Hands hand)
        {
            if (!hand.isFoodHandle)
            {
                if(food)
                    hand.PutItHand(food);
            }
            else
            {
                hand.ReleaseFood();
            }
        }
    }

}