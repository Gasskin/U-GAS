using System;
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
    [MemoryPackUnion(1, typeof(SkillTimelineAnimaClip))]
    [MemoryPackUnion(2, typeof(SkillRecover))]
    [MemoryPackUnion(3, typeof(PushVelocityClip))]
    [MemoryPackUnion(4, typeof(ComboClip))]
    [MemoryPackUnion(5, typeof(AttackInputCacheClip))]
    [MemoryPackUnion(6, typeof(MotionClip))]
    [MemoryPackUnion(7, typeof(SkillHitClip))]
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
        
        public  void FixedTick(float dt, int frame)
        {
            if (_isStart && !_isEnd) 
            {
                OnFixedTick(dt);
            }
        }
        
        public void Interrupt()
        {
            if (_isStart && !_isEnd)
            {
                OnInterrupt();
            }
        }
        
        protected virtual void OnStart()
        {
        }

        protected virtual void OnTick(float dt)
        {
        }
 
        protected virtual void OnFixedTick(float dt)
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