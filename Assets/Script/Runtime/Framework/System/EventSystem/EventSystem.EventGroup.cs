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
                foreach (var pair in _dict)
                {
                    foreach (var listener in pair.Value)
                    {
                        SystemDriver.EventSystem.RemoveListener(pair.Key, listener);
                    }
                }
                _dict.Clear();
            }

            public void AddListener<T>(Action<BaseEventMessage> listener) where T : BaseEventMessage
            {
                var type = typeof(T);
                if (!_dict.ContainsKey(type))
                {
                    _dict.Add(type, new List<Action<BaseEventMessage>>());
                }
                if (!_dict[type].Contains(listener))
                {
                    _dict[type].Add(listener);
                    SystemDriver.EventSystem.AddListener<T>(listener);
                }
                else
                {
                    Debug.LogError($"add same listener: {type}");
                }
            }
        }
    }
}