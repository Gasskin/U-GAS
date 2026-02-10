using Script.Runtime.Framework.ObjectPool;

namespace Script.Runtime.Framework.System
{
    public abstract class BaseEventMessage : IPoolObject
    {
        public abstract void OnRelease();
    }

    public static class EventMessageExtensions
    {
        public static void Send(this BaseEventMessage message)
        {
            SystemDriver.EventSystem.Send(message);
        }
        
        public static void SendNow(this BaseEventMessage message)
        {
            SystemDriver.EventSystem.Send(message);
        }
    }
}