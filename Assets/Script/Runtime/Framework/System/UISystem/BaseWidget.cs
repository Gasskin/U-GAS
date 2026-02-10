using System;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    [DisallowMultipleComponent]
    public abstract class BaseWidget : MonoBehaviour
    {
        [NonSerialized]
        public string DynamicWidgetPath;
        
        protected BaseWindow ParentWindow;
        protected EventSystem.EventGroup EventGroup;

        public void Create(BaseWindow parent)
        {
            ParentWindow = parent;
            EventGroup = ObjectPool.ObjectPool.Get<EventSystem.EventGroup>();
            OnCreate();
        }

        public void Dispose()
        {
            OnDispose();
            ParentWindow = null;
            ObjectPool.ObjectPool.Release(EventGroup);
            EventGroup = null;
        }

        public abstract void OnTick(float dt);
        protected abstract void OnCreate();
        protected abstract void OnDispose();

        public abstract void OnShow();
        public abstract void OnHide();


    }
}