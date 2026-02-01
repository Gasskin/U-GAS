using System;
using System.Collections.Generic;

namespace Script.Runtime.Game.Entity
{
    public class FallState : BaseState
    {
        protected override List<Type> CheckToStates { get; } = new()
        {
            typeof(IdleState),
            typeof(JumpState),
            typeof(RunState),
            typeof(DashState),
        };


        public override void OnEnter()
        {
            StateMachine.PlayAnima(StateMachineComp.StateName_Fall);
        }

        public override void Tick(float dt)
        {
            StateMachine.Fall.ReadInput();
            StateMachine.Turn.CheckAndTurn();
        }

        public override void FixedTick(float dt)
        {
            // var velocity = StateMachine.Fall.CalculateVelocity(StateMachine.Velocity.Velocity);
            // velocity = StateMachine.Movement.CalculateVelocity(velocity, StateMachine.Context.MoveDir.x,
            //     StateMachine.Settings.AirSpeed, StateMachine.Settings.AirAcceleration,
            //     StateMachine.Settings.AirDeceleration);
            // StateMachine.Velocity.AddVelocity(velocity);
        }

        public override void OnExit()
        {
        }

        protected override bool CanEnterTo(BaseState to)
        {
            // switch (to)
            // {
            //     case IdleState:
            //         return StateMachine.Collision.IsGrounded &&
            //                StateMachine.Context.MoveDir.x == 0 &&
            //                StateMachine.Velocity.NoHorizontalVelocity;
            //     case JumpState:
            //         return StateMachine.Context.Jump.IsPressedThisFrame &&
            //                StateMachine.Jump.CanJump;
            //     case RunState:
            //         return StateMachine.Collision.IsGrounded &&
            //                StateMachine.Context.MoveDir.x != 0;
            //     case DashState:
            //         return StateMachine.Dash.CanDash &&
            //                StateMachine.Context.Dash.IsPressedThisFrame;
            // }
            return false;
        }

        public override bool CanEnter()
        {
            return false;
        }
    }
}