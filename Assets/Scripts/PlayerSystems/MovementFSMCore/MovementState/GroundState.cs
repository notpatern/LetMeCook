﻿
using Audio;
using PlayerSystems.MovementFSMCore.MovementContext;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class GroundState : FsmState
    {
        private readonly GroundContext _context;

        public GroundState(GroundContext context, MovementFsmCore fsmCore) : base(context, fsmCore)
        {
            _context = context;
        }

        public override void Update()
        {
            base.Update();
            FrictionManager();
        }

        private void FrictionManager()
        {
            if (fsmCore.jumpHeld)
            {
                fsmCore.rb.drag = _context.jumpHeldFriction;
                return;
            }

            if (fsmCore.Input == new Vector2 (0, 0))
            {
                fsmCore.rb.drag = _context.friction;
            }
            else
            {
                fsmCore.rb.drag = context.drag;
            }
        }

        public override void Jump()
        {
            base.Jump();
            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_PlayerJump);
            fsmCore.coyoteTime = 0f;
        }
    }
}