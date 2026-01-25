using Cysharp.Threading.Tasks;
using MemoryPack;
using UnityEngine;

namespace cfg.Gas
{
    public struct SkillTimelineContext
    {
        public ulong EntityId;
        public int SkillId;
    }
    
    [MemoryPackable]
    [MemoryPackUnion(1, typeof(TestClip))]
    [MemoryPackUnion(2, typeof(SkillTimelineAnimaClip))]
    [MemoryPackUnion(3, typeof(AddGameEffectTo))]
    public abstract partial class SkillTimelineClip
    {
        [HideInInspector]
        public int StartFrame;

        [HideInInspector]
        public int EndFrame;

        protected SkillTimelineContext Context;

        private bool _isStart;
        private bool _isEnd;

        public void Reset(SkillTimelineContext context)
        {
            _isStart = false;
            _isEnd = false;
            Context = context;
        }


        public void Tick(float dt, int frame)
        {
            if (!_isStart && frame >= StartFrame)
            {
                OnStart();
                _isStart = true;
            }
            if (!_isEnd && frame >= EndFrame)
            {
                OnEnd();
                _isEnd = true;
            }
            if (frame >= StartFrame && frame <= EndFrame) 
            {
                OnTick(dt);
            }
        }
        
        protected virtual void OnStart()
        {
        }

        protected virtual void OnTick(float dt)
        {
        }

        protected virtual void OnEnd()
        {
        }

        public virtual void OnInterrupt()
        {
        }
    }
}