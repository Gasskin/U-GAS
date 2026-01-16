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

        private bool _isStart;
        private bool _isEnd;

        public void Reset()
        {
            _isStart = false;
            _isEnd = false;
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