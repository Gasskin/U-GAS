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
        public bool IsActive { get; private set; } = false;

        public void Tick(float dt)
        {
            if (!IsActive)
            {
                return;
            }
            Window.Tick(dt);
        }

        public void Create(BaseUIOpenData openData = null)
        {
            OpenData = openData;
            Window.Create(this);
            Window.Show();
        }

        public void Dispose()
        {
            Window.Hide();
            Window.Dispose();
        }

        public void Show()
        {
            if (IsActive)
            {
                return;
            }
            IsActive = true;
            Window.gameObject.SetActive(true);
            Window.Show();
        }

        public void Hide()
        {
            if (!IsActive)
            {
                return;
            }
            IsActive = false;
            Window.gameObject.SetActive(false);
            Window?.Hide();
        }

        public void OnRelease()
        {
            OpenData?.Release();
            OpenData = null;
            Window = null;
            IsActive = false;
            Uid = 0;
            Config = null;
        }
    }
}