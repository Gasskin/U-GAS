using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

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
    private readonly LinkedList<EntityComp> _updateComps = new();
    private readonly LinkedList<EntityComp> _lateUpdateComps = new();
    private readonly LinkedList<EntityComp> _fixedUpdateComps = new();

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
        var first = _updateComps.First;
        while (first != null)
        {
            first.Value.Tick(dt);
            first = first.Next;
        }
    }

    public void LateTick(float dt)
    {
        if (!IsInitialized)
        {
            return;
        }
        var first = _lateUpdateComps.First;
        while (first != null)
        {
            first.Value.LateTick(dt);
            first = first.Next;
        }
    }

    public void FixedTick(float dt)
    {
        if (!IsInitialized)
        {
            return;
        }
        var first = _fixedUpdateComps.First;
        while (first != null)
        {
            first.Value.FixedTick(dt);
            first = first.Next;
        }
    }

    public T AddComp<T>(T comp) where T : EntityComp
    {
        var p = comp.Priority;
        if (comp.Priority <= 0)
        {
            throw new ArgumentOutOfRangeException($"组件权重异常：{typeof(T).Name}");
        }
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
            Insert(_updateComps);
        }
        if (comp.NeedLateTick)
        {
            if (!_needLateTick)
            {
                _needLateTick = true;
                _system.RegisterLateUpdate(this);
            }
            Insert(_lateUpdateComps);
        }
        if (comp.NeedFixedTick)
        {
            if (!_needFixedTick)
            {
                _needFixedTick = true;
                _system.RegisterFixedUpdate(this);
            }
            Insert(_fixedUpdateComps);
        }

        return comp;

        void Insert(LinkedList<EntityComp> link)
        {
            var first = link.First;
            if (first == null)
            {
                link.AddFirst(comp);
            }
            else
            {
                while (first != null)
                {
                    if (first.Value.Priority > comp.Priority)
                    {
                        link.AddBefore(first, comp);
                        return;
                    }
                    first = first.Next;
                }
                link.AddLast(comp);
            }
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
    }

    public bool HasComp<T>(int priority, out T comp) where T : EntityComp
    {
        comp = null;
        if (_comps != null && _comps.Length > priority)
        {
            comp = (T)_comps[priority];
            return true;
        }
        return false;
    }
}