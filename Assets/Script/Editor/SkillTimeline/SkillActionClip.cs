using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class SkillActionClip: PlayableAsset, ITimelineClipAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SkillActionBehaviour>.Create(graph);
        // 获取 Behaviour 实例并赋值
        var behaviour = playable.GetBehaviour();
        return playable;
    }

    public ClipCaps clipCaps => ClipCaps.None;
}