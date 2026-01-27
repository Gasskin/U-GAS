using UnityEngine.Timeline;

namespace Script.Editor
{
    [TrackColor(1f, 1f, 0f)] 
    [TrackClipType(typeof(SkillTimelineClipAsset))]
    public class SkillTimelineTrack : TrackAsset
    {
        protected override void OnCreateClip(TimelineClip clip)
        {
            base.OnCreateClip(clip);

            clip.start = 0f;
            clip.duration = 1 / 60f * 15f;
        }
    }
}