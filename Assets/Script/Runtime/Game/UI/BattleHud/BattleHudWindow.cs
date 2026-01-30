using System;
using Script.Runtime.Framework;
using Script.Runtime.Framework.System;
using UnityEngine.UI;

namespace Script.Runtime.Game.UI
{
    public class BattleHudWindow : BaseWindow
    {
        public static UIConfig Config = new()
        {
            Layer = EUILayer.Normal,
            Path = "Assets/Bundles/UI/BattleHudWindow.prefab",
        };

        
        public override void OnTick(float dt)
        {
        }

        public override void OnCreate()
        {
            EventGroup.AddListener<OnEntityCreateEvent>(OnEntityCreateEvent);
        }
        

        public override void OnDestroy()
        {
        }

        public override void OnShow()
        {
            
        }

        public override void OnHide()
        {
        }
        
        
        private void OnEntityCreateEvent(BaseEventMessage be)
        {
            if (be is not OnEntityCreateEvent e)
            {
                return;
            }
        }
    }
}