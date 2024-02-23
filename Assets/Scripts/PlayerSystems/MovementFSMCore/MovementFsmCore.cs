
using System;
using System.Dynamic;
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
        [SerializeField] private LayerMask isWall;
        [SerializeField] private LayerMask isGround;
        private InputManager _inputManager;
        private FsmState _currentState;
        
        [Header("Player")] 
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform orientation;

        [Header("Wall Run Check")] 
        [SerializeField] private float wallCheckDistance;
        private Vector2 _playerWalkingInputs;
        private bool _wallRight;
        private bool _wallLeft;
        private RaycastHit _wallRightHit;
        private RaycastHit _wallLeftHit;
        private RaycastHit _wallHit;
        
        private void Start()
        {
            _inputManager = GetComponent<InputManager>();
            rb = GetComponent<Rigidbody>();
        }
    
        private void Update()
        {
            GetPlayerWalkingInputs();
            CheckForWall();
        }
        
        private void SwitchState<TState>(Type state, FsmContext context) where TState : FsmState
        {
            _currentState = (TState)Activator.CreateInstance(
                state,
                context
            );
        }

        private bool CanWallRun()
        {
            if (_wallLeft && _playerWalkingInputs == new Vector2(-1, 1))
            {
                return true;
            }

            return _wallRight && _playerWalkingInputs == new Vector2(1, 1);
        }

        private void CheckForWall()
        {
            var position = transform.position;
            var right = orientation.right;
            
            _wallLeft = Physics.Raycast(
                position,
                -right,
                out _wallLeftHit,
                wallCheckDistance,
                isWall
            );
            
            _wallRight = Physics.Raycast(
                position,
                -right,
                out _wallRightHit,
                wallCheckDistance,
                isWall
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
            _playerWalkingInputs = _inputManager.wasd.ReadValue<Vector2>();
        }
    }
}

