using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    // Open -> Close
    // 如果被全屏界面遮挡 -> Hide
    // 遮挡恢复 -> Show
    [DisallowMultipleComponent]
    public abstract class BaseWindow : MonoBehaviour
    {
    #region Serialize
        [FoldoutGroup("BaseWindow")]
        public List<BaseWidget> StaticWidgets = new();
    #endregion
        
        public Canvas Canvas { get; private set; }
        public int Depth { get; private set; }

        public UILogic Logic;
        
        public EventSystem.EventGroup EventGroup;

        public abstract void OnTick(float dt);
        public abstract void OnOpen();
        public abstract void OnClose();
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
            if (StaticWidgets.Contains(widget))
            {
                Debug.LogError("添加重复Widgets");
                return;
            }
            StaticWidgets.Add(widget);
            widget.OnOpen();
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