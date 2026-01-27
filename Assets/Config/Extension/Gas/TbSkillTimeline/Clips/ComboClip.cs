using System;
using System.Collections.Generic;
using MemoryPack;
using Script.Runtime.Framework.System;

namespace cfg.Gas
{
    [Serializable]
    [MemoryPackable]
    public partial class ComboClip : SkillTimelineClip
    {
        public StringGameTag Combo;

        protected override void OnStart()
        {
            if (SystemDriver.EntitySystem.HasEntity(Context.EntityId, out var entity) &&
                entity.HasComp(EntityComp.Priority_Gas, out GasComp owner))
            {
                owner.GameTagController.AddTag(Combo);
            }
        }

        protected override void OnEnd()
        {
            if (SystemDriver.EntitySystem.HasEntity(Context.EntityId, out var entity) &&
                entity.HasComp(EntityComp.Priority_Gas, out GasComp owner))
            {
                owner.GameTagController.RemoveTag(Combo);
            }
        }

        public override void OnInterrupt()
        {
            OnEnd();
        }
    }
}