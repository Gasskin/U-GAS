using System;
using MemoryPack;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity.StateMachineComp;
using UnityEngine;

namespace cfg.Gas
{
    [Serializable]
    [MemoryPackable]
    public partial class MotionClip : SkillTimelineClip
    {
        public Vector2 Velocity;

        private StateMachineComp _stateMachine;

        protected override void OnStart()
        {
            SystemDriver.EntitySystem.HasComp(Context.EntityId, out _stateMachine);
        }

        protected override void OnFixedTick(float dt)
        {
            if (_stateMachine != null)
            {
                var turn = _stateMachine.Turn.IsFacingRight ? 1 : -1;
                _stateMachine.Velocity.AddVelocity(Velocity * turn);
            }
        }
    }
}