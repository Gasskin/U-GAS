using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.UI;

namespace Script.Runtime.Game
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
            // await SystemDriver.PlayerDataSystem.CreatePlayer();
            SystemDriver.UISystem.OpenWindow(BattleHudWindow.Config);
        
            _isInitialize = true;
            await UniTask.Yield();
        }
    }
}