
using System;
using System.Dynamic;
using PlayerSystems.MovementFSMCore.DataClass;
using PlayerSystems.MovementFSMCore.MovementState;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace PlayerSystems.MovementFSMCore
{
    public class MovementFsmCore : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private LayerMask isGround;
        private InputManager _inputManager;
        private FsmState _currentState;
        public FsmWallRunData wallRunData;
        public FsmAirData airData;
        
        [Header("Player")] 
        public Rigidbody rb;
        public Transform orientation;
        public Vector2 playerWalkingInputs;
        
        private void Start()
        {
            _inputManager = GetComponent<InputManager>();
            rb = GetComponent<Rigidbody>();
        }
    
        private void Update()
        {
            GetPlayerWalkingInputs();
            _currentState.Update();
        }
        
        public void SwitchState<TState>(Type state, FsmContext context) where TState : FsmState
        {
            _currentState = (TState)Activator.CreateInstance(
                state,
                context,
                this
            );
        }
        
        public bool Grounded()
        {
            return Physics.Raycast(
                rb.position,
                Vector3.down,
                1.2f,
                isGround
            );
        }

        private void GetPlayerWalkingInputs()
        {
            playerWalkingInputs = _inputManager.wasd.ReadValue<Vector2>();
        }
    }
}

