using cfg.Gas;
using Script.Runtime.Game.ColliderSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Script.Editor
{
    public class SkillTimelineBehaviour: PlayableBehaviour
    {
        public cfg.Gas.SkillTimelineClip SkillClip;

#if UNITY_EDITOR
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (SkillClip is not SkillAttackHitClip hit)
            {
                return;
            }

            DebugHitBoxMono.TimelineShowBoxHit(hit.PosOffset, hit.Size, hit.Angle);
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            DebugHitBoxMono.ClearSingleBoxHit();
        }
#endif
    }
}