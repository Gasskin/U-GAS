using Script.Runtime.Framework.System;
using Script.Runtime.Game.UI.BattleHud;

namespace Script.Runtime.Game.System
{
    public class InitProcedure: BaseProcedure
    {
        public override void Enter()
        {
            SystemDriver.UISystem.OpenWindow(BattleHudWindow.Config);
        }

        public override void Tick(float dt)
        {
            SystemDriver.ProcedureSystem.ChangeProcedure<BattleProcedure>();
        }

        public override void Exit()
        {
        }
    }
}
