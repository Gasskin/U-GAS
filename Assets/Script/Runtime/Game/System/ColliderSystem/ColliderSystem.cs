using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity;
using UnityEngine;

namespace Script.Runtime.Game.System
{
    public class ColliderSystem : BaseSystem
    {
        public const string LayerHurtBody = "HurtBody";

        private Dictionary<Collider2D, ulong> _hurt2Entity = new();
        private List<Collider2D> _results = new(32);


        public override async UniTask Initialize()
        {
            await UniTask.Yield();
        }

        public override void Destroy()
        {
        }

        public void RegisterHurt(Collider2D hurt, ulong entity)
        {
            _hurt2Entity.Add(hurt, entity);
        }

        public void UnRegisterHurt(Collider2D hurt, ulong entity)
        {
            _hurt2Entity.Remove(hurt);
        }

        public void BoxCast2D(ECamp campFilter, Vector2 center, Vector2 size, float angle)
        {
            var filter = new ContactFilter2D();
            filter.useTriggers = false;
            filter.useLayerMask = true;
            filter.layerMask = LayerMask.GetMask(LayerHurtBody);
            Physics2D.OverlapBox(center, size, angle, filter, _results);
        }
    }
}