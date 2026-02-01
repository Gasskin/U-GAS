using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Runtime.Game.Entity
{
    public class IdleState : BaseState
    {
        protected override List<Type> CheckToStates { get; } = new()
        {
            typeof(RunState),
        };

        public override void OnEnter()
        {
            StateMachine.PlayAnima(StateMachineComp.StateName_Idle);
        }

        public override void Tick(float dt)
        {
            StateMachine.Turn.CheckAndTurn();
        }

        public override void FixedTick(float dt)
        {
            StateMachine.Velocity.AddVelocity(new Vector2(0, -2f));
        }

        public override void OnExit()
        {
        }

        protected override bool CanEnterTo(BaseState to)
        {
            switch (to)
            {
                case RunState:
                    return StateMachine.Context.MoveDir.x != 0;
            }
            return false;
        }
    
    
        public override bool CanEnter()
        {
            return false;
        }
    }
}