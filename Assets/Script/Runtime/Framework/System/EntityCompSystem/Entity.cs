using System;
using System.Collections.Generic;

public class Entity : IPoolObject
{
    public ulong Id { get; private set; }

    public int Index;
    public int UpdateIndex;
    public int LateUpdateIndex;
    public int FixedUpdateIndex;

    public bool NeedUpdate;
    public bool NeedLateUpdate;
    public bool NeedFixedUpdate;
    
    public bool IsValid => Id > 0;
    
    private List<EntityComp> _comps = new(16);
    private Dictionary<int, int> _priority2Index = new();
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
        UpdateIndex = 0;
        LateUpdateIndex = 0;
        FixedUpdateIndex = 0;
        NeedUpdate = false;
        NeedLateUpdate = false;
        NeedFixedUpdate = false;
        _system = null;
        
        for (int i = 0; i < _comps.Count; i++)
        {
            _comps[i]?.OnRemove();
        }
        _comps.Clear();
        _updateComps.Clear();
        _lateUpdateComps.Clear();
        _fixedUpdateComps.Clear();
        _priority2Index.Clear();
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
        if (_priority2Index.ContainsKey(p))
        {
            throw new ArgumentOutOfRangeException($"添加重复组件：{typeof(T).Name}");
        }
        comp.Entity = this;
        _comps.Add(comp);
        _priority2Index.Add(p, _comps.Count - 1);
        comp.OnAdd();

        if (comp.NeedUpdate)
        {
            if (!NeedUpdate)
            {
                NeedUpdate = true;
                _system.RegisterUpdate(this);
            }
            Insert(_updateComps);
        }
        if (comp.NeedLateUpdate)
        {
            if (!NeedLateUpdate)
            {
                NeedLateUpdate = true;
                _system.RegisterLateUpdate(this);
            }
            Insert(_lateUpdateComps);
        }
        if (comp.NeedFixedUpdate)
        {
            if (!NeedFixedUpdate)
            {
                NeedFixedUpdate = true;
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
        if (_priority2Index.TryGetValue(priority,out var index))
        {
            comp = _comps[index];
            return true;
        }
        return false;
    }
}