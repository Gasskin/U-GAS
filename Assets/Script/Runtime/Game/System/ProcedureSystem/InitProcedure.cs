using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.UI.BattleHud;
using Script.Runtime.Game.UI.EnemyHud;

namespace Script.Runtime.Game.System
{
    public class InitProcedure: BaseProcedure
    {
        private EventSystem.EventGroup _eventGroup;

        private bool _isOpen;
        
        public override void Enter()
        {
            _isOpen = false;
            _eventGroup = ObjectPool.Get<EventSystem.EventGroup>();
            _eventGroup.AddListener<OnUIOpenEvent>(OnUIOpenEvent);
            
            SystemDriver.UISystem.OpenWindow(EnemyHudWindow.Config);
            SystemDriver.UISystem.OpenWindow(BattleHudWindow.Config);
        }



        public override void Tick(float dt)
        {
            if (!_isOpen)
            {
                return;
            }
            SystemDriver.ProcedureSystem.ChangeProcedure<BattleProcedure>();
        }

        public override void Exit()
        {
            _isOpen = false;
            ObjectPool.Release(_eventGroup);
        }
        
        private void OnUIOpenEvent(BaseEventMessage obj)
        {
            if (obj is not OnUIOpenEvent e)
            {
                return;
            }
            if (e.Config == EnemyHudWindow.Config)
            {
                _isOpen = true;
            }
        }
    }
}
