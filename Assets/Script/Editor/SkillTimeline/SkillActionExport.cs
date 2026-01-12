using MemoryPack;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class SkillActionExport
{
    [MenuItem("Assets/Battle/SkillAction/ExportOne")]
    public static void ExportOne()
    {
        if (Selection.activeObject is not TimelineAsset timeline)
        {
            return;
        }
        var tracks = timeline.GetOutputTracks();
        foreach (var track in tracks)
        {
            if (track is not SkillActionTrack)
            {
                continue;
            }
            var clips = track.GetClips();
            foreach (var clip in clips)
            {
                if (clip.asset is SkillActionClip c)
                {
                    var t = new SkillActionClipData();
                    t.Action = c.Action;
                    byte[] bin = MemoryPackSerializer.Serialize(t);
                    var test = MemoryPackSerializer.Deserialize<SkillActionClipData>(bin);
                }
            }
        }
    }
}