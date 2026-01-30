using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;

namespace Script.Runtime.Framework.System
{
    public class Entity : IPoolObject
    {
        public ulong Id { get; private set; }

        // 在All Entities列表中的序号
        public int Index;

        // 在Update Entities列表中的序号
        public int TickIndex;

        // 在LateUpdate Entities列表中的序号
        public int LateTickIndex;

        // 在FixedUpdate Entities列表中的序号
        public int FixedTickIndex;

        private bool _needTick;
        private bool _needLateTick;
        private bool _needFixedTick;

        public bool IsValid => Id > 0;

        public bool IsInitialized { get; private set; } = false;

        private EntityComp[] _comps = new EntityComp[32];
        private List<EntityComp> _updateComps = new(32);
        private List<EntityComp> _lateUpdateComps = new(32);
        private List<EntityComp> _fixedUpdateComps = new(32);

        private EntitySystem _system;

        public void Init(ulong id, EntitySystem inSystem)
        {
            Id = id;
            _system = inSystem;
        }

        public void Destroy()
        {
            Id = 0;
            IsInitialized = false;
            Index = 0;
            TickIndex = -1;
            LateTickIndex = -1;
            FixedTickIndex = -1;
            _needTick = false;
            _needLateTick = false;
            _needFixedTick = false;
            _system = null;

            for (int i = _comps.Length - 1; i >= 0; i--)
            {
                _comps[i]?.Destroy();
            }
            _comps = null;
            _updateComps.Clear();
            _lateUpdateComps.Clear();
            _fixedUpdateComps.Clear();
        }

        public void OnRelease()
        {
        }

        public void Tick(float dt)
        {
            if (!IsInitialized)
            {
                return;
            }
            for (int i = 0; i < _updateComps.Count; i++)
            {
                _updateComps[i].OnTick(dt);
            }
        }

        public void LateTick(float dt)
        {
            if (!IsInitialized)
            {
                return;
            }
            for (int i = 0; i < _lateUpdateComps.Count; i++)
            {
                _lateUpdateComps[i].OnTick(dt);
            }
        }

        public void FixedTick(float dt)
        {
            if (!IsInitialized)
            {
                return;
            }
            for (int i = 0; i < _fixedUpdateComps.Count; i++)
            {
                _fixedUpdateComps[i].OnTick(dt);
            }
        }

        public T AddComp<T>(T comp) where T : EntityComp
        {
            var p = SystemDriver.EntitySystem.GetPriority<T>();
            if (p <= 0)
            {
                throw new ArgumentOutOfRangeException($"组件权重异常：{typeof(T).Name}");
            }
            comp.SetPriority(p);
            comp.Entity = this;
            if (_comps.Length <= p)
            {
                var capacity = _comps.Length;
                while (capacity <= p)
                {
                    capacity *= 2;
                }
                var newComps = new EntityComp[capacity];
                for (int i = 0; i < _comps.Length; i++)
                {
                    newComps[i] = _comps[i];
                }
                _comps = newComps;
            }
            _comps[p] = comp;

            if (comp.NeedTick)
            {
                if (!_needTick)
                {
                    _needTick = true;
                    _system.RegisterUpdate(this);
                }
                Insert(_updateComps, comp);
            }
            if (comp.NeedLateTick)
            {
                if (!_needLateTick)
                {
                    _needLateTick = true;
                    _system.RegisterLateUpdate(this);
                }
                Insert(_lateUpdateComps, comp);
            }
            if (comp.NeedFixedTick)
            {
                if (!_needFixedTick)
                {
                    _needFixedTick = true;
                    _system.RegisterFixedUpdate(this);
                }
                Insert(_fixedUpdateComps, comp);
            }

            return comp;

            void Insert(List<EntityComp> insertTo, EntityComp insert)
            {
                if (insertTo.Count <= 0)
                {
                    insertTo.Add(insert);
                    return;
                }

                for (int i = 0; i < insertTo.Count; i++)
                {
                    var to = insertTo[i];
                    if (to.Priority > insert.Priority)
                    {
                        insertTo.Insert(i, insert);
                        return;
                    }
                }
                
                insertTo.Add(insert);
            }
        }

        public async UniTask Initialize()
        {
            for (int i = 0; i < _comps.Length; i++)
            {
                if (_comps[i] == null)
                {
                    continue;
                }
                await _comps[i].Initialize();
            }
            IsInitialized = true;

            var msg = Pool<OnEntityCreateEvent>.Get();
            msg.EntityId = Id;
            msg.Send();
        }

        public bool HasComp<T>(out T comp) where T : EntityComp
        {
            comp = null;
            var priority = SystemDriver.EntitySystem.GetPriority<T>();
            if (priority <= 0) 
            {
                return false;
            }
            if (_comps != null && _comps.Length > priority)
            {
                comp = (T)_comps[priority];
                return true;
            }
            return false;
        }
    }
}