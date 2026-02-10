using System;
using Script.Runtime.Framework.System;
using UnityEngine.UI;

namespace Script.Runtime.Game.UI
{
    public class TestPopupWindow: BaseWindow
    {
        public static UIConfig Config = new()
        {
            Layer = EUILayer.Normal,
            FullScreen = false,
            Path = "Assets/Bundles/UI/TestPopupWindow.prefab",
            CanMultiSpawn = true,
        };

        public Button Close;
        
        protected override void OnTick(float dt)
        {
            
        }

        protected override void OnCreate()
        {
            Close.onClick.AddListener(CloseWindow);
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
