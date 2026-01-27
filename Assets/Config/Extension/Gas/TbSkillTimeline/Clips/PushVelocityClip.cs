using System;
using MemoryPack;
using Script.Runtime.Framework.System;
using Script.Runtime.Game;
using UnityEngine;

namespace cfg.Gas
{
    [Serializable]
    [MemoryPackable]
    public partial class PushVelocityClip : SkillTimelineClip
    {
        public Vector2 Velocity;

        protected override void OnStart()
        {
            if (SystemDriver.EntitySystem.HasEntity(Context.EntityId, out var entity) &&
                entity.HasComp(EntityComp.Priority_StateMachine, out StateMachineComp stateMachine))
            {
                var mult = stateMachine.Turn.IsFacingRight ? 1 : -1;
                stateMachine.Velocity.AddVelocity(Velocity * mult);
            }
        }
    }
}
