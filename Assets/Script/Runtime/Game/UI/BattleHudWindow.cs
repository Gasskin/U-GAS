using System;
using Script.Runtime.Framework.System;

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

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }
    }
}