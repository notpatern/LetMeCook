using UnityEngine;

namespace PlayerSystems.Camera
{
    public class MovePlayerCam : MonoBehaviour
    {
        public Transform camHolder;
    
        void Update()
        {
            transform.position = camHolder.position;
        }
    }
}