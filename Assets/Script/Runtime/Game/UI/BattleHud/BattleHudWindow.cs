using Script.Runtime.Framework.System;

namespace Script.Runtime.Game.UI.BattleHud
{
    public class BattleHudWindow : BaseWindow
    {
        public static UIConfig Config = new()
        {
            Layer = EUILayer.Normal,
            Path = "Assets/Bundles/UI/BattleHud/BattleHudWindow.prefab",
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

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }
    }
}