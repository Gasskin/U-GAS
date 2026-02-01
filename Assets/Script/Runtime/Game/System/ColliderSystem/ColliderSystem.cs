using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity;
using UnityEngine;

namespace Script.Runtime.Game.System
{


    public partial class ColliderSystem : BaseSystem
    {
        public const string LayerHurtBody = "HurtBody";

        private Dictionary<Collider2D, ulong> _hurt2Entity = new();
        private List<Collider2D> _results = new(32);
        private List<ulong> _entityRepeated = new();


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
        
    }
}