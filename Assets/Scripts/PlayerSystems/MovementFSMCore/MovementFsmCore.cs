using System;
using Player.Input;
using PlayerSystems.MovementFSMCore.DataClass;
using PlayerSystems.MovementFSMCore.MovementContext;
using PlayerSystems.MovementFSMCore.MovementState;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;


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
        public FsmDashData dashData;

        public new Transform camera;

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

        private void FixedUpdate()
        {
            currentState.FixedUpdate();
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
            if (Grounded() && currentState.GetType() == typeof(AirState))
            {
                SwitchState<GroundState>(typeof(GroundState), new GroundContext(groundData, true, true));
            }
            else if (!Grounded() && currentState.GetType() == typeof(GroundState))
            {
                SwitchState<AirState>(typeof(AirState), new AirContext(airData, 0f, currentState.context.canJump, currentState.context.canDash));
            }
        }

        private bool Grounded()
        {
            return Physics.Raycast(
                rb.position,
                Vector3.down,
                1.1f,
                isGround
            );
        }
        
        public void OnMovementInputEvent(Vector2 input)
        {
            this.Input = input;
        }

        public void OnJumpInputEvent()
        {
            if (!currentState.context.canJump)
            {
                return;
            }
            
            currentState.Jump();
        }

        public void OnDashInputEvent()
        {
            if (!currentState.context.canDash)
            {
                return;
            }
            
            currentState.Dash();
        }
    }
}

