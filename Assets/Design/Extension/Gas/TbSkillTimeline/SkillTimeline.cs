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