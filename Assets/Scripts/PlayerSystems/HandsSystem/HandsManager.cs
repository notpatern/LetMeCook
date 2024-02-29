using System;
using Player.HandSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

namespace PlayerSystems.HandsSystem
{
    [Serializable]
    public class HandsManager
    {
        [SerializeField] private Hands LeftHand;
        [SerializeField] private Hands RightHand;
        
        public void UseHand(GameObject food, HandsType handsType)
        {
            switch (handsType)
            {
                case HandsType.NONE:
                    Debug.LogError("This should not happen but does.");
                    break;
                case HandsType.LEFT:
                    PerformHandAction(food, LeftHand);
                    break;
                case HandsType.RIGHT:
                    PerformHandAction(food, RightHand);
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