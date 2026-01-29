using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using UnityEngine;

namespace Script.Runtime.Framework
{
    public partial class EventSystem : BaseSystem, ITickSystem
    {
        private Dictionary<Type, List<Action<IEventMessage>>> _listeners = new();
        private List<IEventMessage> _waitMessages = new();

        public override async UniTask Initialize()
        {
            await UniTask.Yield();
        }

        public override void Destroy()
        {
        }

        public void Tick(float dt)
        {
            for (int i = 0; i < _waitMessages.Count; i++)
            {
                var msg = _waitMessages[i];
                var type = msg.GetType();
                if (_listeners.TryGetValue(type, out var listeners))
                {
                    for (int j = 0; j < listeners.Count; j++)
                    {
                        listeners[i]?.Invoke(msg);
                    }
                }
            }
            _waitMessages.Clear();
        }

        private void AddListener<T>(Action<IEventMessage> listener)
        {
            var type = typeof(T);
            if (!_listeners.TryGetValue(type, out var listeners))
            {
                listeners = new List<Action<IEventMessage>>();
                _listeners.Add(type, listeners);
            }
            listeners.Add(listener);
        }

        private void RemoveListener(Type type, Action<IEventMessage> listener)
        {
            if (_listeners.TryGetValue(type, out var listeners))
            {
                listeners.Remove(listener);
            }
        }

        public void Send(IEventMessage message)
        {
            _waitMessages.Add(message);
        }
    }
}