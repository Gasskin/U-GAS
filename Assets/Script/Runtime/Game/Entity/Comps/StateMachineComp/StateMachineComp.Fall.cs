using UnityEngine;

namespace Script.Runtime.Game.Entity.StateMachineComp
{
    public class StateMachineFall
    {
        private StateMachineComp _stateMachine;


        public void Initialize(StateMachineComp stateMachine)
        {
            _stateMachine = stateMachine;
        }
    
        public void ReadInput()
        {
        }

        public Vector2 CalculateVelocity(Vector2 inVelocity)
        {
            // var gravityMultiplier = _stateMachine.Settings.FallGravityMultiplayer;
            // if (Mathf.Abs(inVelocity.y) <= _stateMachine.Settings.HangJumpVelocity)
            // {
            //     gravityMultiplier = _stateMachine.Settings.HangJumpGravityMultiplayer;
            // }
            // else if (_stateMachine.Context.Jump.IsHolding)
            // {
            //     gravityMultiplier = _stateMachine.Settings.HoldJumpGravityMultiplayer;
            // }
            // inVelocity.y -= _stateMachine.Settings.Gravity * gravityMultiplier * Time.fixedDeltaTime;
            //
            // inVelocity.y = Mathf.Clamp(inVelocity.y, _stateMachine.Settings.MaxFallSpeed, _stateMachine.Settings.MaxUpwardSpeed);

            return inVelocity;
        }
    }

    public partial class StateMachineComp
    {
        public StateMachineFall Fall = new();
    }
}