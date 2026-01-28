using System;
using MemoryPack;
using Script.Runtime.Framework.System;
using Script.Runtime.Game;
using Script.Runtime.Game.Entity.StateMachineComp;

namespace cfg.Gas
{
    /// <summary>
    /// 后摇
    /// </summary>
    [Serializable]
    [MemoryPackable]
    public partial class SkillRecover : SkillTimelineClip
    {
        protected override void OnStart()
        {
            if (SystemDriver.EntitySystem.HasEntity(Context.EntityId, out var entity) &&
                entity.HasComp(EntityComp.Priority_StateMachine, out StateMachineComp stateMachine))
            {
                stateMachine.SkillSpell.ChangeSkillStagePriority(ESkillStagePriority.None);
            }
        }
    }
}