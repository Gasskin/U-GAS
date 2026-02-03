using System;
using System.Collections.Generic;
using MemoryPack;
using Script.Runtime.Framework;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using Script.Runtime.Framework.System.GameEffect;
using Script.Runtime.Game.Entity;
using Script.Runtime.Game.System;
using UnityEngine;
using UnityEngine.Pool;

namespace cfg.Gas
{
    /// <summary>
    /// 命中前
    /// </summary>
    public class PreSkillAttackHitEvent : BaseEventMessage
    {
        public ulong EntityId;
        public int SkillId;
        public List<BoxHitResult> HitResult;

        public static void Send(ulong eId, int skillId, List<BoxHitResult> hitResult)
        {
            var e = ObjectPool.Get<PreSkillAttackHitEvent>();
            e.EntityId = eId;
            e.SkillId = skillId;
            e.HitResult = ListPool<BoxHitResult>.Get();
            e.HitResult.AddRange(hitResult);
            e.Send();
        }

        public override void OnRelease()
        {
            EntityId = 0;
            SkillId = 0;
            ListPool<BoxHitResult>.Release(HitResult);
            HitResult = null;
        }
    }

    /// <summary>
    /// 命中伤害前
    /// </summary>
    public class PreSkillAttackHitDamageEvent : BaseEventMessage
    {
        public ulong EntityId;
        public int SkillId;
        public ulong TargetEntityId;
        public int DamageEffectId;
        public GameEffectContext GameEffectContext;

        public static void SendNow(ulong eId, int skillId, ulong targetEntityId, int damageEffectId,
            GameEffectContext context)
        {
            var e = ObjectPool.Get<PreSkillAttackHitDamageEvent>();
            e.EntityId = eId;
            e.SkillId = skillId;
            e.TargetEntityId = targetEntityId;
            e.DamageEffectId = damageEffectId;
            e.GameEffectContext = context;
            e.SendNow();
        }

        public override void OnRelease()
        {
            EntityId = 0;
            SkillId = 0;
            TargetEntityId = 0;
            DamageEffectId = 0;
            GameEffectContext = null;
        }
    }

    /// <summary>
    /// 命中后
    /// </summary>
    public class PostSkillAttackHitEvent : BaseEventMessage
    {
        public ulong EntityId;
        public int SkillId;
        public List<BoxHitResult> HitResult;

        public static void Send(ulong eId, int skillId, List<BoxHitResult> hitResult)
        {
            var e = ObjectPool.Get<PostSkillAttackHitEvent>();
            e.EntityId = eId;
            e.SkillId = skillId;
            e.HitResult = ListPool<BoxHitResult>.Get();
            e.HitResult.AddRange(hitResult);
            e.Send();
        }

        public override void OnRelease()
        {
            EntityId = 0;
            SkillId = 0;
            ListPool<BoxHitResult>.Release(HitResult);
            HitResult = null;
        }
    }

    [Serializable]
    [MemoryPackable]
    public partial class SkillAttackHitClip : SkillTimelineClip
    {
        public Vector2 PosOffset;
        public Vector2 Size;
        public float Angle;
        public ECampRelation Relation;

        public List<int> HitDamage = new();

        protected override void OnStart()
        {
            if (!SystemDriver.EntitySystem.HasComp(Context.EntityId, out GameObjectComp gameObjectComp))
            {
                Debug.LogError($"no GameObjectComp found, entityId: {Context.EntityId}");
                return;
            }
            if (!SystemDriver.EntitySystem.HasComp(Context.EntityId, out StateMachineComp stateMachineComp))
            {
                Debug.LogError($"no StateMachineComp found, entityId: {Context.EntityId}");
                return;
            }
            if (!SystemDriver.EntitySystem.HasComp(Context.EntityId, out GasComp sourceGas))
            {
                Debug.LogError($"no GasComp found, entityId: {Context.EntityId}");
                return;
            }

            var rootPos = gameObjectComp.View.transform.position;
            var pos = PosOffset + new Vector2(rootPos.x, rootPos.y) * stateMachineComp.Turn.RightMult;
            var hitResult = ListPool<BoxHitResult>.Get();
            SystemDriver.ColliderSystem.BoxHit(Context.EntityId, Relation, pos, Size, Angle, hitResult);

            // pre hit
            PreSkillAttackHitEvent.Send(Context.EntityId, Context.SkillId, hitResult);

            for (int i = 0; i < hitResult.Count; i++)
            {
                if (!SystemDriver.EntitySystem.HasComp(hitResult[i].EntityId, out GasComp targetGas))
                {
                    Debug.LogError($"no GasComp found, entityId: {hitResult[i].EntityId}");
                    continue;
                }
                for (int j = 0; j < HitDamage.Count; j++)
                {
                    var context = ObjectPool.Get<GameEffectContext>();
                    context.SkillAttackHitCollider = hitResult[i].Collider;
                 
                    // pre apply damage
                    PreSkillAttackHitDamageEvent.SendNow(Context.EntityId, Context.SkillId,hitResult[i].EntityId,
                        HitDamage[j], context);
                    
                    sourceGas.ApplyGameEffectTo(HitDamage[j], context, targetGas);
                }
            }

            // post hit
            PostSkillAttackHitEvent.Send(Context.EntityId, Context.SkillId, hitResult);


            ListPool<BoxHitResult>.Release(hitResult);
        }
    }
}