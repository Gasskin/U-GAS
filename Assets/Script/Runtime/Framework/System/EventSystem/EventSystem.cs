using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;

namespace Script.Runtime.Framework
{
    public partial class EventSystem : BaseSystem, ITickSystem
    {
        private Dictionary<Type,List<EventGroup>> _eventGroups = new();
        private List<BaseEventMessage> _waitMessages = new();

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
                if (_eventGroups.TryGetValue(type, out var groups))
                {
                    for (int j = 0; j < groups.Count; j++)
                    {
                        groups[j].Trigger(type, msg);
                    }
                }
                ObjectPool.ObjectPool.Release(msg);
            }
            _waitMessages.Clear();
        }

        private void RegisterEventGroup(Type type, EventGroup group)
        {
            if (!_eventGroups.TryGetValue(type,out var groups))
            {
                groups = new List<EventGroup>();
                _eventGroups.Add(type, groups);
            }
            groups.Add(group);
        }

        private void UnRegisterEventGroup(Type type, EventGroup group)
        {
            if (_eventGroups.TryGetValue(type,out var groups))
            {
                groups.Remove(group);
            }
        }

        public void Send(BaseEventMessage message)
        {
            _waitMessages.Add(message);
        }
    }
}