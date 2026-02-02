using System;
using MemoryPack;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity;
using Script.Runtime.Game.System;
using UnityEngine;
using UnityEngine.Pool;

namespace cfg.Gas
{
    [Serializable]
    [MemoryPackable]
    public partial class SkillAttackHitClip : SkillTimelineClip
    {
        public Vector2 PosOffset;
        public Vector2 Size;
        public float Angle;
        public ECampRelation Relation;

        protected override void OnStart()
        {
            if (SystemDriver.EntitySystem.HasComp(Context.EntityId, out GameObjectComp gameObjectComp) &&
                SystemDriver.EntitySystem.HasComp(Context.EntityId,out StateMachineComp stateMachineComp))
            {
                var rootPos = gameObjectComp.View.transform.position;
                var pos = PosOffset + new Vector2(rootPos.x, rootPos.y) * stateMachineComp.Turn.RightMult;
                var hitResult = ListPool<BoxHitResult>.Get();
                SystemDriver.ColliderSystem.BoxHit(Context.EntityId, Relation, pos, Size, Angle, hitResult);
                ListPool<BoxHitResult>.Release(hitResult);
            }
        }
    }
}