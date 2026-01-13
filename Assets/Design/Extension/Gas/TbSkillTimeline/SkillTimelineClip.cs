using MemoryPack;
using UnityEngine;

namespace cfg.Gas
{
    [MemoryPackable]
    [MemoryPackUnion(1, typeof(TestClip))]
    [MemoryPackUnion(2, typeof(AddGameEffectTo))]
    public abstract partial class SkillTimelineClip
    {
        [HideInInspector]
        public int StartFrame;
        [HideInInspector]
        public int EndFrame;

        public virtual void OnStart()
        {
            
        }

        public virtual void OnTick(float dt)
        {
            
        }

        public virtual void OnEnd()
        {
            
        }

        public virtual void OnInterrupt()
        {
            
        }
    }
}