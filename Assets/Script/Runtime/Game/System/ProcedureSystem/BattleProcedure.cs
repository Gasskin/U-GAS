using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;

namespace Script.Runtime.Game.System
{
    public class BattleProcedure: BaseProcedure
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
            await SystemDriver.PlayerDataSystem.CreatePlayer();
            _isInitialize = true;
            await UniTask.Yield();
        }
    }
}