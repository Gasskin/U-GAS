using System;
using MemoryPack;

namespace cfg.Gas
{
    [Serializable]
    [MemoryPackable]
    public partial class ChangeSkillStagePriority : SkillTimelineClip
    {
        public ESkillStagePriority ChangeTo;
        

        protected override void OnStart()
        {
            if (SystemDriver.EntitySystem.HasEntity(Context.EntityId, out var entity) &&
                entity.HasComp(EntityComp.Priority_StateMachine, out StateMachineComp stateMachine))
            {
                stateMachine.SkillSpell.ChangeSkillStagePriority(ChangeTo);
            }
        }
    }
}