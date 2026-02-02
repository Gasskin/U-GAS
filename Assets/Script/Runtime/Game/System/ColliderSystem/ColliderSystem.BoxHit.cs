using System;
using System.Collections.Generic;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.ColliderSystem;
using Script.Runtime.Game.Entity;
using UnityEngine;

namespace Script.Runtime.Game.System
{
    public struct BoxHitResult
    {
        public Collider2D Collider;
        public ulong EntityId;
    }

    public partial class ColliderSystem
    {
        public void BoxHit(ulong sourceCampEntity, ECampRelation relation, Vector2 pos, Vector2 size, float angle,
            List<BoxHitResult> hitResult)
        {
            _entityRepeated.Clear();
            hitResult.Clear();

            if (!SystemDriver.EntitySystem.HasComp(sourceCampEntity, out CampComp sourceCamp))
            {
                return;
            }

            var filter = new ContactFilter2D();
            filter.useTriggers = false;
            filter.useLayerMask = true;
            filter.layerMask = LayerMask.GetMask(LayerHurtBody);
            
            var count = Physics2D.OverlapBox(pos, size, angle, filter, _results);
#if UNITY_EDITOR
            DebugHitBoxMono.ShowBoxHit(pos, size, angle);
#endif
            
            for (int i = 0; i < count; i++)
            {
                var result = _results[i];
                if (!_hurt2Entity.TryGetValue(result, out var entityId))
                {
                    continue;
                }
                if (_entityRepeated.Contains(entityId))
                {
                    continue;
                }
                if (!SystemDriver.EntitySystem.HasComp(entityId, out CampComp hitCamp))
                {
                    continue;
                }
                _entityRepeated.Add(entityId);
                if (sourceCamp.RelationTo(hitCamp) == relation)
                {
                    hitResult.Add(new BoxHitResult()
                    {
                        Collider = result,
                        EntityId = entityId,
                    });
                }
            }
        }
    }
}