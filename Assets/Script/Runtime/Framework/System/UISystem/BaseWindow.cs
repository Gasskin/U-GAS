using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    // Create + Show -> ( Hide -> Show -> ) Hide -> Dispose
    [DisallowMultipleComponent]
    public abstract class BaseWindow : MonoBehaviour
    {
    #region Serialize
        [FoldoutGroup("BaseWindow"), SerializeField]
        private List<BaseWidget> _staticWidgets = new();
    #endregion

        public bool IsValid => Logic != null;
        public Canvas Canvas { get; private set; }
        public int Depth { get; private set; }

        protected UILogic Logic;

        public EventSystem.EventGroup EventGroup;

        private List<BaseWidget> _dynamicWidgets = new();

        public void Tick(float dt)
        {
            OnTick(dt);
            for (int i = 0; i < _staticWidgets.Count; i++)
            {
                _staticWidgets[i].OnTick(dt);
            }
            for (int i = 0; i < _dynamicWidgets.Count; i++)
            {
                _dynamicWidgets[i].OnTick(dt);
            }
        }

        public void Create(UILogic logic)
        {
            Logic = logic;
            EventGroup = ObjectPool.ObjectPool.Get<EventSystem.EventGroup>();
            
            OnCreate();
            for (int i = 0; i < _staticWidgets.Count; i++)
            {
                _staticWidgets[i].Create(this);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _staticWidgets.Count; i++)
            {
                _staticWidgets[i].Dispose();
            }
            for (int i = 0; i < _dynamicWidgets.Count; i++)
            {
                _dynamicWidgets[i].Dispose();
                SystemDriver.GameObjectPoolSystem.Release(_dynamicWidgets[i].DynamicWidgetPath,
                    _dynamicWidgets[i].gameObject);
            }
            _dynamicWidgets.Clear();
            OnDispose();
            
            Logic = null;
            ObjectPool.ObjectPool.Release(EventGroup);
            EventGroup = null;
        }

        public void Show()
        {
            OnShow();
            for (int i = 0; i < _staticWidgets.Count; i++)
            {
                _staticWidgets[i].OnShow();
            }
            for (int i = 0; i < _dynamicWidgets.Count; i++)
            {
                _dynamicWidgets[i].OnShow();
            }
        }

        public void Hide()
        {
            for (int i = 0; i < _staticWidgets.Count; i++)
            {
                _staticWidgets[i].OnHide();
            }
            for (int i = 0; i < _dynamicWidgets.Count; i++)
            {
                _dynamicWidgets[i].OnHide();
            }
            OnHide();
        }

        protected abstract void OnTick(float dt);
        protected abstract void OnCreate();
        protected abstract void OnDispose();
        protected abstract void OnShow();

        protected abstract void OnHide();

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public void OnDepthChange(int depth)
        {
            Depth = depth;
        }

        public async UniTask<BaseWidget> AddDynamicWidget(string widgetPath, Transform parent)
        {
            // 等一帧，避免在LifeCycle期间增删Widget
            await UniTask.Yield();
            var prefab = await SystemDriver.GameObjectPoolSystem.GetAsync(widgetPath, parent);
            if (prefab == null)
            {
                throw new NullReferenceException($"widget asset is null: {widgetPath}");
            }
            if (!IsValid)
            {
                SystemDriver.GameObjectPoolSystem.Release(widgetPath, prefab);
                return null;
            }
            var widget = prefab.GetComponent<BaseWidget>();
            if (widget == null)
            {
                throw new NullReferenceException($"widget is null: {widgetPath}");
            }
            widget.DynamicWidgetPath = widgetPath;
            widget.Create(this);
            if (Logic.IsActive)
            {
                widget.OnShow();
            }
            _dynamicWidgets.Add(widget);
            return widget;
        }

        public void RemoveDynamicWidget(BaseWidget widget)
        {
            if (!_dynamicWidgets.Remove(widget))
            {
                return;
            }
            widget.OnHide();
            widget.Dispose();
            SystemDriver.GameObjectPoolSystem.Release(widget.DynamicWidgetPath, widget.gameObject);
        }

        protected void CloseWindow()
        {
            SystemDriver.UISystem.CloseWindow(Logic.Uid);
        }
    }
}