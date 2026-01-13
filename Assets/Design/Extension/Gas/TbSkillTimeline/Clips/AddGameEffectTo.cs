using MemoryPack;
using UnityEngine;

namespace cfg.Gas
{
    [MemoryPackable]
    public partial class AddGameEffectTo: SkillTimelineClip
    {
        public int Id;

        public override void OnStart()
        {
            var design = SystemDriver.DesignSystem.Tables.TbGameEffect.GetOrDefault(Id);
            if (design != null) 
            {
                Debug.LogError(design.Backup);
            }
        }
    }
}