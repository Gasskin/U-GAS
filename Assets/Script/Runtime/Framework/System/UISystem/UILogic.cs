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

        public BaseWindow Window;
        public ulong Uid;
        public bool IsActive = false;

        public void Tick(float dt)
        {
            Window?.OnTick(dt);
        }

        public void Open()
        {
            Window?.OnOpen();
        }

        public void Close()
        {
            Window?.OnClose();
        }

        public void OnRelease()
        {
            Window = null;
            Uid = 0;
            IsActive = false;
        }
    }
}