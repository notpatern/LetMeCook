using System;
using PlayerSystems.MovementFSMCore.MovementContext;
using UnityEngine;
using System.Collections;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class DashState : FsmState
    {
        private readonly DashContext _context;
        private Vector3 _dashDirection;
        
        public DashState(DashContext context, MovementFsmCore fsmCore) : base(context, fsmCore)
        {
            this._context = context;
        }

        public override void Init()
        {
            base.Init();
            fsmCore.rb.drag = 0;
            fsmCore.rb.useGravity = context.useGravity;
            
            float[] fovValues = { fsmCore.cameraData.dashFov, fsmCore.cameraData.dashFovTimeToSet };
            fsmCore.onFovChange.TriggerEvent(fovValues);
            
            _dashDirection = fsmCore.camera.forward;
            fsmCore.rb.AddForce(_dashDirection * (fsmCore.rb.velocity.magnitude + _context.dashForce), ForceMode.Impulse);
        }

        public override void FixedUpdate()
        {
            DashTimer();
            switch (_context.dashDuration)
            {
                case > 0 when _context.dashDuration <= 0.05:
                    fsmCore.rb.drag = context.drag;
                    break;
                case < 0:
                    ExitDash();
                    break;
            }
        }

        private void DashTimer()
        {
            _context.dashDuration -= Time.fixedDeltaTime;
        }

        private void ExitDash()
        {
            float[] fovValues = { fsmCore.cameraData.defaultFov, fsmCore.cameraData.defaultFovTimeToSet };
            fsmCore.onFovChange.TriggerEvent(fovValues);
            fsmCore.SwitchState<AirState>(typeof(AirState), new AirContext(fsmCore.airData, 0f, context.canJump, false));
        }

        public override void Dash()
        {
        }

        public override void Jump()
        {
        }
    }
}