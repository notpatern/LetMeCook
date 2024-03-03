
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSystems.Camera
{
    public class PlayerCam : MonoBehaviour
    {

        public float sensX;
        public float sensY;

        public Transform orientation;
        public new Transform camera;

        float _xRotation;
        float _yRotation;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            float mouseY = Mouse.current.delta.ReadValue().y * sensY * Time.deltaTime * ControlOptions.ControlOptionsManagement.s_Instance.GetMouseSensitivity();
            float mouseX = Mouse.current.delta.ReadValue().x * sensX * Time.deltaTime * ControlOptions.ControlOptionsManagement.s_Instance.GetMouseSensitivity();

            _yRotation += mouseX;
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            camera.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        }
    }
}