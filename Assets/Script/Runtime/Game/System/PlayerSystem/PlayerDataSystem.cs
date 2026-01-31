using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity;

namespace Script.Runtime.Game
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
            // if (!SystemDriver.EntityRootSystem.TryGetHeroFirstCell(out var index, out var cell))
            // {
            //     return;
            // }

            _level = 10;

            Player = await EntityHero.Create(new EntityHero()
            {
                Level = _level,
                HeroId = 1001,
            });

            await UniTask.Yield();
        }
    }
}