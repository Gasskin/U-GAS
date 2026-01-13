using System;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;

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

    private List<EntityComp> _comps = new(32);
    private LinkedList<EntityComp> _updateComps = new();
    private LinkedList<EntityComp> _lateUpdateComps = new();
    private LinkedList<EntityComp> _fixedUpdateComps = new();

    private EntitySystem _system;

    public void Init(ulong id, EntitySystem system)
    {
        Id = id;
        _system = system;
    }

    public void Destroy()
    {
        Id = 0;
        Index = 0;
        TickIndex = -1;
        LateTickIndex = -1;
        FixedTickIndex = -1;
        _needTick = false;
        _needLateTick = false;
        _needFixedTick = false;
        _system = null;

        for (int i = 0; i < _comps.Count; i++)
        {
            _comps[i]?.OnRemove();
        }
        _comps.Clear();
        _updateComps.Clear();
        _lateUpdateComps.Clear();
        _fixedUpdateComps.Clear();
    }

    public void OnRelease()
    {
    }

    public void Tick(float dt)
    {
        var first = _updateComps.First;
        while (first != null)
        {
            first.Value.Tick(dt);
            first = first.Next;
        }
    }

    public void LateTick(float dt)
    {
        var first = _lateUpdateComps.First;
        while (first != null)
        {
            first.Value.Tick(dt);
            first = first.Next;
        }
    }

    public void FixedTick(float dt)
    {
        var first = _fixedUpdateComps.First;
        while (first != null)
        {
            first.Value.Tick(dt);
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
        if (_comps.Capacity <= p)
        {
            var newComps = new List<EntityComp>(2 * _comps.Capacity);
            for (int i = 0; i < _comps.Count; i++)
            {
                newComps[i] = _comps[i];
            }
            _comps = newComps;
        }
        _comps[p] = comp;
        comp.OnAdd();

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
                        break;
                    }
                    first = first.Next;
                }
            }
        }
    }

    public bool HasComp(int priority, out EntityComp comp)
    {
        comp = null;
        if (_comps != null && _comps.Count > priority)
        {
            comp = _comps[priority];
            return true;
        }
        return false;
    }
}