using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Runtime.Game
{
    public class RunState : BaseState
    {
        protected override List<Type> CheckToStates { get; } = new()
        {
            typeof(JumpState),
            typeof(IdleState),
        };

        public override void OnEnter()
        {
            StateMachine.PlayAnima(StateMachineComp.StateName_Run);
        }

        public override void Tick(float dt) 
        {
            StateMachine.Turn.CheckAndTurn();
        }

        public override void FixedTick(float dt)
        {
            var velocity = StateMachine.Movement.CalculateVelocity(StateMachine.Velocity.Velocity,
                StateMachine.Settings.RunSpeed, StateMachine.Settings.RunAcceleration,
                StateMachine.Settings.RunDeceleration);
        
            StateMachine.Velocity.AddVelocity(velocity);
        }


        public override void OnExit()
        {
        }

        protected override bool CanEnterTo(BaseState to)
        {
            switch (to)
            {
                case IdleState:
                    return StateMachine.Context.MoveDir == Vector2Int.zero && StateMachine.Velocity.NoVelocity;
            }
            return false;
        }


        public override bool CanEnter()
        {
            return false;
            ;
        }
    }
}