using System;
using UnityEngine;

namespace Player.HandSystem
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
            }
        }
    }

}