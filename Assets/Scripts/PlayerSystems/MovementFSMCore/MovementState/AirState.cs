﻿using Audio;
using PlayerSystems.MovementFSMCore.MovementContext;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace PlayerSystems.MovementFSMCore.MovementState
{
    public class AirState : FsmState
    {
        private readonly AirContext _context;
        
        [Header("Wall Run Check")] 
        private LayerMask _isWall;
        private bool _wallRight;
        private bool _wallLeft;
        private RaycastHit _wallRightHit;
        private RaycastHit _wallLeftHit;
        private RaycastHit _wallHit;
        private bool _wallRightTilt;
        
        public AirState(AirContext airContext, MovementFsmCore fsmCore) : base(airContext, fsmCore)
        {
            this._context = airContext;
        }
        public override void Init()
        {
            base.Init();
            _context.Init();
            _isWall = LayerMask.GetMask("isWall");
        }

        public override void Update()
        {
            base.Update();
            if (fsmCore.coyoteTime >= 0)
            {
                DecreaseCoyoteTime();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            IsWallNear();
            if (!CanWallRun() || !WallRunExited() || !fsmCore.canWallRun)
            {
                return;
            }
            fsmCore.SwitchState<WallRunState>(typeof(WallRunState), new WallRunContext(fsmCore.wallRunData, _wallHit, _wallRightTilt, _context.canJump));
        }

        private bool WallRunExited()
        {
            if (_context.wallRunExitTime <= 0f)
            {
                return true;
            }
            _context.wallRunExitTime -= Time.deltaTime;
            return false;
        }

        public override void Jump()
        {
            if (fsmCore.coyoteTime > 0)
            {
                base.Jump();
                fsmCore.coyoteTime = 0f;
                return;
            }

            if (jumpLeniency)
            {
                fsmCore.coyoteTime = 0f;
                return;
            }

            DoubleJump();
        }

        private void DoubleJump() {
            fsmCore.coyoteTime = 0f;

            var vel = fsmCore.rb.velocity;
            fsmCore.rb.velocity = new Vector3(vel.x - GetPercentage(vel.x, _context.doubleJumpDeceleration), 0, vel.z - GetPercentage(vel.z, _context.doubleJumpDeceleration));

            fsmCore.mono.StartCoroutine(JumpImpulse());

            AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_PlayerDoubleJump);

            fsmCore.rb.AddForce(Vector3.up * _context.doubleJumpForce, ForceMode.Impulse);
            context.canJump = false;
        }

        private float GetPercentage(float startValue, float percetage) {
            return (startValue * percetage) / 100;
        }

        IEnumerator JumpImpulse() {
            float[] zTilt = { fsmCore.cameraData.doubleJumpTilt + fsmCore.rb.velocity.magnitude / fsmCore.cameraData.doubleJumpSpeedMultiplier, 0, fsmCore.cameraData.doubleJumpTiltTimeToSet };
            fsmCore.onTiltChange.TriggerEvent(zTilt);

            yield return new WaitForSeconds(fsmCore.cameraData.doubleJumpTiltTimeToSet);

            float[] newzTilt = { 0, 0, fsmCore.cameraData.doubleJumpTiltTimeReset };
            fsmCore.onTiltChange.TriggerEvent(newzTilt);
        }
        
        private bool CanWallRun()
        {
            if (fsmCore.Stamina <= 0)
            {
                return false;
            }
            
            bool canWallRun = false;

            if (_wallLeft && (fsmCore.Input is { y: > 0 } || fsmCore.Input is not { x: 0 }))
            {
                _wallHit = _wallLeftHit;
                _wallRightTilt = false;
                canWallRun = true;
            }
            if (_wallRight && (fsmCore.Input is { y: > 0 } || fsmCore.Input is not { x: 0 }))
            {
                _wallHit = _wallRightHit;
                _wallRightTilt = true;
                canWallRun = true;
            }

            if (_context.previousWallInfo.colliderInstanceID == 0)
            {
                return canWallRun;
            }
            
            if (_context.previousWallInfo.colliderInstanceID == _wallHit.colliderInstanceID &&
                _context.previousWallInfo.point.y - _context.restartWallRunFalloffDistance <= _wallHit.point.y)
            {
                canWallRun = false;
            }

            return canWallRun;
        }

        private void IsWallNear()
        {
            var position = fsmCore.rb.position;
            var right = fsmCore.orientation.right;
            
            _wallLeft = Physics.Raycast(
                position,
                -right,
                out _wallLeftHit,
                _context.wallCheckDistance,
                _isWall
            );
            
            _wallRight = Physics.Raycast(
                position,
                right,
                out _wallRightHit,
                _context.wallCheckDistance,
                _isWall
            );
        }
    }
}