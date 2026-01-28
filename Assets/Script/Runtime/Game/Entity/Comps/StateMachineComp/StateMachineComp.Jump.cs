using System;
using UnityEngine;

namespace Script.Runtime.Game.Entity.StateMachineComp
{
    public class StateMachineJump
    {
        public event Action OnMultiJump;
    
        // 最新速度是下降，并且计算速度前是上升
        public bool CanFall => _stateMachine.Velocity.Velocity.y < 0 && _isUp && !_hasPressed;
        public bool CanJump => _jumpCount > 0;

    
    
        private StateMachineComp _stateMachine;

        // 计算速度前是上升状态
        private bool _isUp;

        private bool _isHolding;
        private bool _hasPressed;
        private bool _hasReleased;

        private int _jumpCount;

        public void Initialize(StateMachineComp stateMachine)
        {
            _stateMachine = stateMachine;

            // _jumpCount = _stateMachine.Settings.CanJumpCount;
            // _stateMachine.Collision.OnGroundTouched += (() =>
            // {
            //     _jumpCount = _stateMachine.Settings.CanJumpCount;
            // });
        }

        public void ReadInput()
        {
            if (_stateMachine.Context.Jump.IsPressedThisFrame)
            {
                _hasPressed = true;
            }
            if (_stateMachine.Context.Jump.IsReleasedThisFrame)
            {
                _hasReleased = true;
            }
            _isHolding = _stateMachine.Context.Jump.IsHolding;
        }

        public Vector2 CalculateVelocity(Vector2 inVelocity)
        {
            // if (inVelocity.y > 0)
            // {
            //     _isUp = true;
            // }
            //
            // var jump = _hasPressed && _jumpCount > 0;
            // var multiJump = jump && _jumpCount != _stateMachine.Settings.CanJumpCount;
            //
            // if (jump)
            // {
            //     _jumpCount--;
            //     inVelocity.y = _stateMachine.Settings.MaxJumpVelocity;
            // }
            //
            // if (multiJump)
            // {
            //     OnMultiJump?.Invoke();
            // }
            //
            // // 如果松开跳跃键，设置为最小跳跃速度
            // if (_hasReleased)
            // {
            //     inVelocity.y = Mathf.Min(inVelocity.y, _stateMachine.Settings.MinJumpVelocity);
            // }
            //
            // _hasPressed = false;
            // _hasReleased = false;

            return inVelocity;
        }
    

        public void Exit()
        {
            _isUp = false;
        }
    }

    public partial class StateMachineComp
    {
        public StateMachineJump Jump = new();
    }
}