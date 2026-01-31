using System;
using System.Collections.Generic;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using UnityEngine;

namespace Script.Runtime.Framework
{
    public partial class EventSystem
    {
        public class EventGroup : IPoolObject
        {
            private Dictionary<Type, List<Action<BaseEventMessage>>> _dict = new();

            public void OnRelease()
            {
                foreach (var type in _dict.Keys)
                {
                    SystemDriver.EventSystem.UnRegisterEventGroup(type, this);
                }
                _dict.Clear();
            }

            public void AddListener<T>(Action<BaseEventMessage> listener) where T : BaseEventMessage
            {
                var type = typeof(T);
                if (!_dict.TryGetValue(type, out var listeners))
                {
                    listeners = new List<Action<BaseEventMessage>>();
                    _dict[type] = listeners;
                    SystemDriver.EventSystem.RegisterEventGroup(type, this);
                }
                if (!listeners.Contains(listener))
                {
                    listeners.Add(listener);
                }
                else
                {
                    Debug.LogError($"添加重复事件");
                }
            }

            public void Trigger(Type type, BaseEventMessage msg)
            {
                if (_dict.TryGetValue(type, out var listeners))
                {
                    for (int i = 0; i < listeners.Count; i++)
                    {
                        listeners[i]?.Invoke(msg);
                    }
                }
            }
        }
    }
}