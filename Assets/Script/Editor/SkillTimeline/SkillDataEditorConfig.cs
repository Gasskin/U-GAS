using System.Collections.Generic;
using MemoryPack;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "SkillDataEditorConfig", menuName = "Battle/Skill/SkillDataEditorConfig")]
public class SkillDataEditorConfig: ScriptableObject
{
    public int Id;
    public List<TimelineAsset> TimelineAssets;

    [Button]
    public void Export()
    {
        var skillData = new SkillData();
        skillData.Id = Id;

        foreach (var timelineAsset in TimelineAssets)
        {
            var tracks = timelineAsset.GetOutputTracks();
            foreach (var track in tracks)
            {
                var clips = track.GetClips();
                foreach (var clip in clips)
                {
                    if (clip.asset is SkillActionClip skillActionClip)
                    {
                        skillData.ClipDatas.Add(new SkillActionClipData()
                        {
                            StartFrame = 1,
                            EndFrame = 2,
                            Action = skillActionClip.Action,
                        });
                    }
                }
            }
        }

        var bin = MemoryPackSerializer.Serialize(skillData);
        var o = MemoryPackSerializer.Deserialize<SkillData>(bin);
    }
}