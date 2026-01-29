using Script.Runtime.Framework.ObjectPool;

namespace Script.Runtime.Framework.System
{
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
        public bool IsActive { get; private set; } = true;
        // public bool IsPaused { get; private set; } = false;

        public void Tick(float dt)
        {
            if (!IsActive)
            {
                return;
            }
            Window?.OnTick(dt);
        }

        public void Open()
        {
            Window.Logic = this;
            Window?.OnOpen();
        }

        public void Close()
        {
            Window?.OnClose();
        }

        public void Show()
        {
            if (IsActive)
            {
                return;
            }
            IsActive = true;
            Window.gameObject.SetActive(true);
            Window?.OnShow();
        }

        public void Hide()
        {
            if (!IsActive)
            {
                return;
            }
            IsActive = false;
            Window.gameObject.SetActive(false);
            Window?.OnHide();
        }


        public void OnRelease()
        {
            Window.Logic = null;
            Window = null;
            IsActive = true;
            Uid = 0;
            Config = null;
        }
    }
}