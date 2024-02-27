using System;
using Player.Input;
using PlayerSystems.MovementFSMCore.DataClass;
using PlayerSystems.MovementFSMCore.MovementContext;
using PlayerSystems.MovementFSMCore.MovementState;
using UnityEngine;


namespace PlayerSystems.MovementFSMCore
{
    public class MovementFsmCore : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private LayerMask isGround;
        [HideInInspector] public FsmState currentState;
        public FsmWallRunData wallRunData;
        public FsmAirData airData;
        public FsmGroundData groundData;
        
        [Header("Player")] 
        public Rigidbody rb;
        public Transform orientation;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            currentState = new GroundState(new GroundContext(groundData), this);
        }
    
        private void Update()
        {
            currentState.Update();
        }
        
        public void SwitchState<TState>(Type state, FsmContext context) where TState : FsmState
        {
            currentState = (TState)Activator.CreateInstance(
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
    }
}

