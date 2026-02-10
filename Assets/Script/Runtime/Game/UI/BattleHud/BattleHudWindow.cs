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

        protected override void OnTick(float dt)
        {
        }

        protected override void OnCreate()
        {
        }


        protected override void OnDispose()
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }
    }
}