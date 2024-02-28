using UnityEngine;

namespace Level.LevelDesign
{
    public class MovingPlatform : MonoBehaviour
    {
        public Vector3 lookAtPoint = Vector3.zero;

        public void Update()
        {
            transform.LookAt(lookAtPoint);
        }
    }
}
