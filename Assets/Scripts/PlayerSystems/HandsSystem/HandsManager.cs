using System;
using Player.HandSystem;
using UnityEngine;

namespace PlayerSystems.HandsSystem
{
    [Serializable]
    public class HandsManager
    {
        public void LoadHand(GameObject food, HandsType handsType)
        {
            switch (handsType)
            {
                case HandsType.NONE:
                    Debug.LogError("This should not happen but does.");
                    break;
                case HandsType.LEFT:
                    Debug.Log("Left");
                    break;
                case HandsType.RIGHT:
                    Debug.Log("Right");
                    break;
            }
        }
    }

}