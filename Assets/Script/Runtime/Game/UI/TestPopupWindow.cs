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
        
        public override void OnTick(float dt)
        {
            
        }

        public override void OnCreate()
        {
            Close.onClick.AddListener(CloseWindow);
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
    }
}
