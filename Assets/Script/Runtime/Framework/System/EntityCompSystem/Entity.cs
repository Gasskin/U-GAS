using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    private bool needTick;
    private bool needLateTick;
    private bool needFixedTick;

    public bool IsValid => Id > 0;

    public bool IsInitialized { get; private set; } = false;

    private EntityComp[] comps = new EntityComp[32];
    private readonly LinkedList<EntityComp> updateComps = new();
    private readonly LinkedList<EntityComp> lateUpdateComps = new();
    private readonly LinkedList<EntityComp> fixedUpdateComps = new();

    private EntitySystem system;

    public void Init(ulong id, EntitySystem inSystem)
    {
        Id = id;
        system = inSystem;
    }

    public void Destroy()
    {
        Id = 0;
        IsInitialized = false;
        Index = 0;
        TickIndex = -1;
        LateTickIndex = -1;
        FixedTickIndex = -1;
        needTick = false;
        needLateTick = false;
        needFixedTick = false;
        system = null;

        for (int i = 0; i < comps.Length; i++)
        {
            comps[i]?.Destroy();
        }
        comps = null;
        updateComps.Clear();
        lateUpdateComps.Clear();
        fixedUpdateComps.Clear();
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
        var first = updateComps.First;
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
        var first = lateUpdateComps.First;
        while (first != null)
        {
            first.Value.Tick(dt);
            first = first.Next;
        }
    }

    public void FixedTick(float dt)
    {
        if (!IsInitialized)
        {
            return;
        }
        var first = fixedUpdateComps.First;
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
        if (comps.Length <= p)
        {
            var capacity = comps.Length;
            while (capacity <= p)
            {
                capacity *= 2;
            }
            var newComps = new EntityComp[capacity];
            for (int i = 0; i < comps.Length; i++)
            {
                newComps[i] = comps[i];
            }
            comps = newComps;
        }
        comps[p] = comp;

        if (comp.NeedTick)
        {
            if (!needTick)
            {
                needTick = true;
                system.RegisterUpdate(this);
            }
            Insert(updateComps);
        }
        if (comp.NeedLateTick)
        {
            if (!needLateTick)
            {
                needLateTick = true;
                system.RegisterLateUpdate(this);
            }
            Insert(lateUpdateComps);
        }
        if (comp.NeedFixedTick)
        {
            if (!needFixedTick)
            {
                needFixedTick = true;
                system.RegisterFixedUpdate(this);
            }
            Insert(fixedUpdateComps);
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

    public async UniTask Initialize()
    {
        for (int i = 0; i < comps.Length; i++)
        {
            if (comps[i] == null)
            {
                continue;
            }
            await comps[i].Initialize();
        }
        IsInitialized = true;
    }

    public bool HasComp<T>(int priority, out T comp) where T : EntityComp
    {
        comp = null;
        if (comps != null && comps.Length > priority)
        {
            comp = (T)comps[priority];
            return true;
        }
        return false;
    }
}