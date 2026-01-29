using System;
using Script.Runtime.Framework.System;
using UnityEngine.UI;

namespace Script.Runtime.Game.UI
{
    public class TestWindow : BaseWindow
    {
        public static UIConfig Config = new()
        {
            Layer = EUILayer.Normal,
            Path = "Assets/Bundles/UI/TestWindow.prefab",
        };

        public Button Close;
        public Button Pop;
        
        public override void OnTick(float dt)
        {
        }

        public override void OnOpen()
        {
            Close.onClick.AddListener((CloseWindow));
            Pop.onClick.AddListener(((() =>
            {
                SystemDriver.UISystem.OpenWindow(TestPopupWindow.Config);
            })));
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