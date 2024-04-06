
using System;
using ControlOptions;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace PlayerSystems.Camera
{
    public class PlayerCam : MonoBehaviour
    {
        public GameEventScriptableObject onFovChange;
        public GameEventScriptableObject onTiltChange;
        
        public Transform orientation;
        public new Transform camera;
        public Transform camHolder;
        [SerializeField] Transform _handCamera;

        float _xRotation;
        float _yRotation;

        void Awake()
        {
            onFovChange.BindEventAction(DoFov);
            onTiltChange.BindEventAction(DoTilt);
        }

        void Start()
        {
            ControlOptionsManagement.SetCursorIsPlayMode(true);
        }

        void Update()
        {
            float mouseY = Mouse.current.delta.ReadValue().y * 0.01f * ControlOptions.ControlOptionsManagement.s_Instance.GetMouseSensitivity() * 3 * Time.timeScale;
            float mouseX = Mouse.current.delta.ReadValue().x * 0.01f * ControlOptions.ControlOptionsManagement.s_Instance.GetMouseSensitivity() * 3 * Time.timeScale;

            _yRotation += mouseX;
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            camHolder.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        }

        void DoFov(object fovValue)
        {
            var array = (float[])fovValue;
            GetComponent<UnityEngine.Camera>().DOFieldOfView(array[0], array[1]);
            _handCamera.GetComponent<UnityEngine.Camera>().DOFieldOfView(array[0], array[1]);
        }

        void DoTilt(object zTiltValue)
        {
            transform.DOLocalRotate(new Vector3(0, 0, (float)zTiltValue), 0.25f);
            _handCamera.DOLocalRotate(new Vector3(0, 0, (float)zTiltValue), 0.25f);
        }
    }
}