using System;
using Player.Input;
using PlayerSystems.MovementFSMCore.DataClass;
using PlayerSystems.MovementFSMCore.MovementContext;
using PlayerSystems.MovementFSMCore.MovementState;
using UnityEngine;
using UnityEngine.Serialization;


namespace PlayerSystems.MovementFSMCore
{
    public class MovementFsmCore : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private LayerMask isGround;

        [HideInInspector] public FsmState currentState;
        public FsmWallRunData wallRunData;
        public FsmAirData airData;
        public FsmGroundData groundData;

        [Header("Player")] public Rigidbody rb;
        public Transform orientation;

        [HideInInspector] public Vector2 Input { private set; get; }

        private void Start()
        {
            currentState = new GroundState(new GroundContext(groundData), this);
            currentState.Init();
        }
    
        private void Update()
        {
            currentState.Update();
            HandleGroundedState();
        }
        
        public void SwitchState<TState>(Type state, FsmContext context) where TState : FsmState
        {
            currentState = (TState)Activator.CreateInstance(
                state,
                context,
                this
            );
            currentState.Init();
        }

        private void HandleGroundedState()
        {
            if (Grounded() && currentState.GetType() != typeof(GroundState))
            {
                SwitchState<GroundState>(typeof(GroundState), new GroundContext(groundData));
            }
            else
            {
                SwitchState<AirState>(typeof(AirState), new AirContext(airData));
            }
        }

        private bool Grounded()
        {
            return Physics.Raycast(
                rb.position,
                Vector3.down,
                1.3f,
                isGround
            );
        }
        
        public void OnMovementInputEvent(Vector2 input)
        {
            this.Input = input;
        }

        public void OnJumpInputEvent()
        {
            currentState.Jump();
        }
    }
}

