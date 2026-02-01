using System;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;

namespace Script.Runtime.Framework
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
    }
}