using System;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    [DisallowMultipleComponent]
    public abstract class BaseWidget : MonoBehaviour
    {
        public abstract void OnTick(float dt);
        public abstract void OnCreate();
        public abstract void OnDestroy();

        public abstract void OnShow();
        public abstract void OnHide();

        [NonSerialized]
        public BaseWindow ParentWindow;
        
        private void Awake()
        {
            ParentWindow = GetComponentInParent<BaseWindow>();
            ParentWindow.Widgets.Add(this);
        }


    }
}
