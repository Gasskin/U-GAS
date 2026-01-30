using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    // Open -> Close
    // 如果被全屏界面遮挡 -> Hide
    // 遮挡恢复 -> Show
    [DisallowMultipleComponent]
    public abstract class BaseWindow : MonoBehaviour
    {
        public Canvas Canvas { get; private set; }
        public int Depth { get; private set; }

        public UILogic Logic;
        
        public List<BaseWidget> Widgets = new();

        public EventSystem.EventGroup EventGroup;

        public abstract void OnTick(float dt);
        public abstract void OnCreate();
        public abstract void OnDestroy();
        public abstract void OnShow();

        public abstract void OnHide();


        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public void OnDepthChange(int depth)
        {
            Depth = depth;
        }

        protected void AddWidget(BaseWidget widget)
        {
            if (Widgets.Contains(widget))
            {
                Debug.LogError("添加重复Widgets");
                return;
            }
            Widgets.Add(widget);
            widget.OnCreate();
            if (!Logic.IsActive)
            {
                widget.OnHide();
            }
        }

        protected void CloseWindow()
        {
            SystemDriver.UISystem.CloseWindow(Logic.Uid);
        }
    }
}