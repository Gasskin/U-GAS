using System;
using Script.Runtime.Framework.ObjectPool;

namespace Script.Runtime.Framework.System
{
    public abstract class BaseUIOpenData : IPoolObject
    {
        public abstract void Release();
        public abstract void OnRelease();
    }

    public class UILogic : IPoolObject
    {
    #region Uid
        private static ulong _uid = 1;

        public static ulong GetUid()
        {
            if (_uid == ulong.MaxValue)
            {
                _uid = 1;
            }
            return _uid++;
        }
    #endregion

        public UIConfig Config;
        public BaseWindow Window;
        public ulong Uid;
        public BaseUIOpenData OpenData;
        public bool IsActive { get; private set; } = true;

        public void Tick(float dt)
        {
            if (!IsActive)
            {
                return;
            }
            Window.OnTick(dt);
            for (int i = 0; i < Window.StaticWidgets.Count; i++)
            {
                Window.StaticWidgets[i].OnTick(dt);
            }
        }

        public void Open(BaseUIOpenData openData = null)
        {
            OpenData = openData;
            Window.Logic = this;
            Window.EventGroup = Pool<EventSystem.EventGroup>.Get();
            Window.OnOpen();
            for (int i = 0; i < Window.StaticWidgets.Count; i++)
            {
                Window.StaticWidgets[i].ParentWindow = Window;
                Window.StaticWidgets[i].OnOpen();
            }
        }

        public void Close()
        {
            for (int i = 0; i < Window.StaticWidgets.Count; i++)
            {
                Window.StaticWidgets[i].OnClose();
                Window.StaticWidgets[i].ParentWindow = null;
            }
            Window.OnClose();
            Window.Logic = null;
            Pool<EventSystem.EventGroup>.Release(Window.EventGroup);
            Window.EventGroup = null;
        }

        public void Show()
        {
            if (IsActive)
            {
                return;
            }
            IsActive = true;
            Window.gameObject.SetActive(true);
            Window.OnShow();
            for (int i = 0; i < Window.StaticWidgets.Count; i++)
            {
                Window.StaticWidgets[i].OnShow();
            }
        }

        public void Hide()
        {
            if (!IsActive)
            {
                return;
            }
            IsActive = false;
            Window.gameObject.SetActive(false);
            for (int i = 0; i < Window.StaticWidgets.Count; i++)
            {
                Window.StaticWidgets[i].OnHide();
            }
            Window?.OnHide();
        }

        public void OnRelease()
        {
            OpenData?.Release();
            OpenData = null;
            Window = null;
            IsActive = true;
            Uid = 0;
            Config = null;
        }
    }
}