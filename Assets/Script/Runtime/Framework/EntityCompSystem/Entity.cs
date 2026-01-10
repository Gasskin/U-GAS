using System.Collections.Generic;

public class Entity : IPoolObject
{
    public ulong Id { get; private set; }

    private List<EntityComp> _comps;
    private LinkedList<EntityComp> _updateComps = new();
    private LinkedList<EntityComp> _lateUpdateComps = new();
    private LinkedList<EntityComp> _fixedUpdateComps = new();

    public void Init(ulong id, int compNum)
    {
        Id = id;
        _comps = new List<EntityComp>(compNum);
    }
    
    public void Destroy()
    {
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

    public void AddComp(EntityComp comp, bool update, bool lateUpdate, bool fixedUpdate)
    {
        comp.Entity = this;
        _comps[comp.Priority] = comp;
        comp.OnAdd();
        
        if (update)
        {
            Insert(_updateComps);
        }
        if (lateUpdate)
        {
            Insert(_lateUpdateComps);
        }
        if (fixedUpdate)
        {
            Insert(_fixedUpdateComps);
        }

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

    public bool HasComp(int priority)
    {
        return _comps.Count > priority && _comps[priority] != null;
    }

}