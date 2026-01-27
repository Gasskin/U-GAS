using System.Collections.Generic;
using MemoryPack;

namespace cfg.Gas
{
    [MemoryPackable]
    public partial class SkillTimeline
    {
        public int Id;
        public int StartFrame = int.MaxValue;
        public int EndFrame = int.MinValue;
        public List<SkillTimelineClip> Clips = new();

        public void Reset(SkillTimelineContext context)
        {
            for (int i = 0; i < Clips.Count; i++)
            {
                Clips[i].Reset(context);
            }
        }

        public bool Tick(float dt, int frame)
        {
            if (frame < StartFrame)
            {
                return false;
            }
            if (frame > EndFrame)
            {
                return true;
            }
            for (int i = 0; i < Clips.Count; i++)
            {
                Clips[i].Tick(dt, frame);
            }
            return false;
        }

        public void FixedTick(float dt, int frame)
        {
            if (frame < StartFrame)
            {
                return ;
            }
            if (frame > EndFrame)
            {
                return ;
            }
            for (int i = 0; i < Clips.Count; i++)
            {
                Clips[i].FixedTick(dt, frame);
            }
        }

        public void Interrupt()
        {
            for (int i = 0; i < Clips.Count; i++)
            {
                Clips[i].Interrupt();
            }
        }


#if UNITY_EDITOR
        public void AddClip(SkillTimelineClip clip)
        {
            Clips.Add(clip);
            if (clip.StartFrame < StartFrame)
            {
                StartFrame = clip.StartFrame;
            }
            if (clip.EndFrame > EndFrame)
            {
                EndFrame = clip.EndFrame;
            }
        }
#endif
    }
}