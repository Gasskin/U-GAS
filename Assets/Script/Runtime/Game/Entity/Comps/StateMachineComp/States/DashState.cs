using System;
using System.Collections.Generic;

namespace Script.Runtime.Game
{
    public class DashState : BaseState
    {
        protected override List<Type> CheckToStates { get; } = new()
        {
            typeof(IdleState),
            typeof(RunState),
            typeof(FallState),
        };

        public override void OnEnter()
        {
            StateMachine.PlayAnima(StateMachineComp.StateName_Dash);
        }

        public override void Tick(float dt)
        {
            StateMachine.Dash.ReadInput();
        }

        public override void FixedTick(float dt)
        {
            var velocity = StateMachine.Dash.CalculateVelocity(StateMachine.Velocity.Velocity);

            StateMachine.Velocity.AddVelocity(velocity);
        }

        public override void OnExit()
        {
            StateMachine.Dash.Exit();
        }

        protected override bool CanEnterTo(BaseState to)
        {
            switch (to)
            {
                // case IdleState:
                //     return StateMachine.Dash.CanExit &&
                //            StateMachine.Collision.IsGrounded &&
                //            StateMachine.Context.MoveDir.x == 0;
                // case RunState:
                //     return StateMachine.Dash.CanExit &&
                //            StateMachine.Collision.IsGrounded &&
                //            StateMachine.Context.MoveDir.x != 0;
                // case FallState:
                //     return StateMachine.Dash.CanExit &&
                //            !StateMachine.Collision.IsGrounded;
            }
            return false;
        }

        public override bool CanEnter()
        {
            return false;
        }
    }
}