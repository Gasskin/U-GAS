using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity;
using UnityEngine.Pool;

namespace Script.Runtime.Game.System
{
    public class PlayerDataSystem : BaseSystem
    {
        public Framework.System.Entity Player { get; private set; }

        private int _level;

        public override async UniTask Initialize()
        {
            await UniTask.Yield();
        }

        public override void Destroy()
        {
            if (Player != null)
            {
                SystemDriver.EntitySystem?.DestroyEntity(Player);
            }
        }

        public async UniTask CreatePlayer()
        {
            _level = 10;

            var entityHero = ObjectPool.Get<EntityHero>();
            entityHero.Level = _level;
            entityHero.HeroId = 1001;
            Player = await EntityHero.Create(entityHero);

            await UniTask.Yield();
        }
    }
}