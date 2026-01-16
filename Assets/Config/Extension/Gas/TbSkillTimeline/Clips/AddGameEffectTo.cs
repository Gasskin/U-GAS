using MemoryPack;
using UnityEngine;

namespace cfg.Gas
{
    [MemoryPackable]
    public partial class AddGameEffectTo: SkillTimelineClip
    {
        public int Id;

        protected override void OnStart()
        {
            var design = SystemDriver.ConfigSystem.Tables.TbGameEffect.GetOrDefault(Id);
            if (design != null) 
            {
                Debug.LogError(design.Backup);
            }
        }
    }
}