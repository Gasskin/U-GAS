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
        
        protected override void OnTick(float dt)
        {
        }

        protected override void OnCreate()
        {
            Close.onClick.AddListener((CloseWindow));
            Pop.onClick.AddListener(((() =>
            {
                SystemDriver.UISystem.OpenWindow(TestPopupWindow.Config);
            })));
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