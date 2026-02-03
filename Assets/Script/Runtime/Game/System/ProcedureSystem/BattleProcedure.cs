using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity;

namespace Script.Runtime.Game.System
{
    public class BattleProcedure : BaseProcedure
    {
        private bool _isInitialize;

        public override void Enter()
        {
            _isInitialize = false;
            Initialize().Forget();
        }

        public override void Tick(float dt)
        {
            if (!_isInitialize)
            {
                return;
            }
        }

        public override void Exit()
        {
        }

        private async UniTaskVoid Initialize()
        {
            var entityMonster = ObjectPool.Get<EntityMonster>();
            entityMonster.Level = 10;
            entityMonster.MonsterId = 10001;
            await EntityMonster.Create(entityMonster);
            
            await SystemDriver.PlayerDataSystem.CreatePlayer();
            _isInitialize = true;
            await UniTask.Yield();
        }
    }
}