using Script.Runtime.Framework.System;

namespace Script.Runtime.Framework
{
    public interface IEventMessage
    {
        
    }

    public static class EventMessageExtensions
    {
        public static void Send(this IEventMessage message)
        {
            SystemDriver.EventSystem.Send(message);
        }
    }
}