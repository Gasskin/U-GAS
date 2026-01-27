using System;
using Cysharp.Threading.Tasks;
using MemoryPack;
using Script.Runtime.Framework.System;
using Script.Runtime.Game;
using YooAsset;

namespace cfg.Gas
{
    [MemoryPackable]
    public partial class SkillTimelineAnimaClip : SkillTimelineClip
    {
        public string AnimationName;

        protected override void OnStart()
        {
            if (SystemDriver.EntitySystem.HasEntity(Context.EntityId, out var entity) &&
                entity.HasComp(EntityComp.Priority_StateMachine, out StateMachineComp stateMachine))
            {
                if (stateMachine.IsState<SkillSpellState>(out var state))
                {
                    state.PlayAnima(AnimationName);
                }
            }
        }

        protected override void OnEnd()
        {
        }

        public override void OnInterrupt()
        {
        }
    }
}