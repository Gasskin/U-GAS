using Script.Runtime.Framework;
using Script.Runtime.Framework.System;

namespace Script.Runtime.Game.UI
{
    public class BattleHudWindow : BaseWindow
    {
        public static UIConfig Config = new()
        {
            Layer = EUILayer.Normal,
            Path = "Assets/Bundles/UI/BattleHud/BattleHudWindow.prefab",
        };

        public HpBarWidget HpBar;
        public HpBarWidget MpBar;
        public HpBarWidget ShieldBar;

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