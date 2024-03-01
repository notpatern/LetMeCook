using JetBrains.Annotations;
using UnityEngine;

namespace Level.LevelDesign
{
    [System.Serializable]
    public class MovingPlatformKey
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        
        public float pauseBeforeMoving;
        public float travelTime;

        public MovingPlatformKey(Transform newKeyTransform, float pauseBeforeMoving, float travelTime)
        {
            this.position = newKeyTransform.position;
            this.rotation = newKeyTransform.rotation.eulerAngles;
            this.scale = newKeyTransform.localScale;
            this.pauseBeforeMoving = pauseBeforeMoving;
            this.travelTime = travelTime;
        }
    }
}