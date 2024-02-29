using UnityEngine;

namespace Level.LevelDesign
{
    [System.Serializable]
    public class MovingPlatformKey
    {
        public Vector3 position;
        public Quaternion rotation;
        public float pauseBeforeMoving;
        public float travelTime;

        public MovingPlatformKey(Vector3 position, Quaternion rotation, float pauseBeforeMoving, float travelTime)
        {
            this.position = position;
            this.rotation = rotation;
            this.pauseBeforeMoving = pauseBeforeMoving;
            this.travelTime = travelTime;
        }
    }
}