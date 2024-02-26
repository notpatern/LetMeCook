using UnityEngine;

namespace Player.HandSystem
{
    public class Hands : MonoBehaviour
    {
        HandsType handsType;
    }

    public enum HandsType
    {
        NONE = 0,
        LEFT = 1, RIGHT = 2
    }
}