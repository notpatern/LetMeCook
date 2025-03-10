﻿using ControlOptions;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace PlayerSystems.Camera
{
    public class PlayerCam : MonoBehaviour
    {
        public GameEventScriptableObject onFovChange;
        public GameEventScriptableObject onTiltChange;
        public GameEventScriptableObject onShake;
        
        public Transform orientation;
        public new Transform camera;
        public Transform camHolder;
        [SerializeField] Transform _handCamera;

        private Transform cameraPosition;

        float _xRotation;
        float _yRotation;

        void Awake()
        {
            onFovChange.BindEventAction(DoFov);
            onTiltChange.BindEventAction(DoTilt);
            onShake.BindEventAction(DoShake);
        }

        void Start()
        {
            ControlOptionsManagement.SetCursorIsPlayMode(true);
            cameraPosition = camera.transform;
        }

        void Update()
        {
            float mouseY = Mouse.current.delta.ReadValue().y * 0.01f * ControlOptionsManagement.s_Instance.GetMouseSensitivity() * 3 * Time.timeScale;
            float mouseX = Mouse.current.delta.ReadValue().x * 0.01f * ControlOptionsManagement.s_Instance.GetMouseSensitivity() * 3 * Time.timeScale;

            _yRotation += mouseX;
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            camHolder.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        }

        void DoFov(object fovValue)
        {
            if (camera == null || _handCamera == null) {
                return;
            }

            var array = (float[])fovValue;
            GetComponent<UnityEngine.Camera>().DOFieldOfView(array[0], array[1]);
            _handCamera.GetComponent<UnityEngine.Camera>().DOFieldOfView(array[0], array[1]);
        }

        void DoTilt(object zTiltValue)
        {
            if (camera == null || _handCamera == null) {
                return;
            }

            var array = (float[])zTiltValue;
            transform.DOLocalRotate(new Vector3(array[0], 0, array[1]), array[2]);
            _handCamera.DOLocalRotate(new Vector3(array[0], 0, array[1]), array[2]);
        }

        Tween shake;

        void DoShake(object shakeValue) {
            /// summary:
            /// float duration, float strength, int vibrator, float randomness
            
            if (camera == null || _handCamera == null) {
                return;
            }

            var array = (float[])shakeValue;
            shake.Complete();
            shake = camera.transform.DOShakePosition(array[0], array[1], (int)array[2], array[3], false, true, ShakeRandomnessMode.Harmonic);
        }
    }
}