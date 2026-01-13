using cfg.Gas;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class SkillTimelineClipAsset: PlayableAsset, ITimelineClipAsset
{
    [SerializeReference]
    public cfg.Gas.SkillTimelineClip SkillClip;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SkillTimelineBehaviour>.Create(graph);
        // 获取 Behaviour 实例并赋值
        var behaviour = playable.GetBehaviour();
        return playable;
    }

    public ClipCaps clipCaps => ClipCaps.None;
}