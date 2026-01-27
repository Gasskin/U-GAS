using UnityEngine;

namespace Script.Runtime.Game
{
    public class StateMachineDash
    {
        public bool CanExit => !_hasPressed && _dashTime <= 0f;

        public bool CanDash => _dashCount > 0;

        private StateMachineComp _stateMachine;

        private bool _hasPressed;

        private int _dashCount;

        private float _dashTime;
    
        private float _dashRecoverTime;

        public void Initialize(StateMachineComp machine)
        {
            _stateMachine = machine;
            // _dashCount = _stateMachine.Settings.CanDashCount;
            // _stateMachine.Collision.OnGroundTouched += (() => { _dashCount = _stateMachine.Settings.CanDashCount; });
        }

        public void ReadInput()
        {
            if (_stateMachine.Context.Dash.IsPressedThisFrame)
            {
                _hasPressed = true;
            }
        }

        public Vector2 CalculateVelocity(Vector2 inVelocity)
        {
            // var dash = _hasPressed && _dashCount > 0;
            //
            // if (dash)
            // {
            //     _dashCount--;
            //
            //     _dashTime = _stateMachine.Settings.DashTime;
            // }
            //
            // if (_dashTime > 0)
            // {
            //     var facing = _stateMachine.Turn.IsFacingRight ? 1 : -1;
            //     inVelocity.x = facing * _stateMachine.Settings.DashVelocity;
            //     inVelocity.y = 0f;
            // }
            //
            // _hasPressed = false;

            return inVelocity;
        }

        public void Tick(float dt)
        {
            // if (_dashTime > 0)
            // {
            //     _dashTime -= dt;
            //     return;
            // }
            //
            // if (_dashRecoverTime > 0) 
            // {
            //     _dashRecoverTime -= dt;
            //     if (_dashRecoverTime <= 0)
            //     {
            //         _dashCount = _stateMachine.Settings.CanDashCount;
            //     }
            // }
        }

        public void Exit()
        {
            // _dashRecoverTime = _stateMachine.Settings.DashRecoverTime;
        }
    }

    public partial class StateMachineComp
    {
        public StateMachineDash Dash = new();
    }
}