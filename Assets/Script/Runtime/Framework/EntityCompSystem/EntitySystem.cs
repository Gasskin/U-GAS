using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Pool;

public class EntitySystem : BaseSystem, ITickSystem, IFixedTickSystem, ILateTickSystem
{
    private Dictionary<Type, int> _compPriority = new();

    private HashSet<Type> _updateType = new();
    private HashSet<Type> _lateUpdateType = new();
    private HashSet<Type> _fixedUpdateType = new();

    private Dictionary<ulong, Entity> _id2Entity = new();
    private List<ulong> _updateEntities = new();
    private List<ulong> _lateUpdateEntities = new();
    private List<ulong> _fixedUpdateEntities = new();

    private int _compNum;

    private ulong _entityIdGenerator = 0;

    public override void Initialize()
    {
        _id2Entity.Clear();

        var assembly = GetType().Assembly;

        var compTypes = assembly
            .GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                typeof(EntityComp).IsAssignableFrom(t) &&
                t.GetCustomAttribute<EntityCompAttribute>() != null
            ).ToList();

        _compNum = compTypes.Count;
        _compPriority.Clear();
        _updateType.Clear();
        _lateUpdateType.Clear();
        _fixedUpdateType.Clear();
        foreach (var compType in compTypes)
        {
            var attr = compType.GetCustomAttribute<EntityCompAttribute>();
            _compPriority[compType] = attr.Priority;
            if (attr.Update)
            {
                _updateType.Add(compType);
            }
            if (attr.LateUpdate)
            {
                _lateUpdateType.Add(compType);
            }
            if (attr.FixedUpdate)
            {
                _fixedUpdateType.Add(compType);
            }
        }
    }

    public override void Close()
    {
        var entities = _id2Entity.Keys.ToList();
        foreach (var id in entities)
        {
            DestroyEntity(id);
        }
    }

    public void Tick(float dt)
    {
        foreach (var id in _updateEntities)
        {
            _id2Entity[id]?.Tick(dt);
        }
    }

    public void LateTick(float dt)
    {
        foreach (var id in _lateUpdateEntities)
        {
            _id2Entity[id]?.Tick(dt);
        }
    }

    public void FixedTick(float dt)
    {
        foreach (var id in _fixedUpdateEntities)
        {
            _id2Entity[id]?.Tick(dt);
        }
    }


    public Entity CreateEntity()
    {
        if (_entityIdGenerator >= ulong.MaxValue)
        {
            _entityIdGenerator = 0;
        }
        var id = _entityIdGenerator++;
        if (_id2Entity[id] != null)
        {
            throw new ArgumentOutOfRangeException($"EntityId 重复");
        }
        var e = Pool<Entity>.Get();
        e.Init(id, _compNum);
        _id2Entity[id] = e;
        return e;
    }

    public void DestroyEntity(Entity e)
    {
        DestroyEntity(e.Id);
    }

    public void DestroyEntity(ulong eId)
    {
        if (_id2Entity.TryGetValue(eId, out var entity))
        {
            entity.Destroy();
            _id2Entity.Remove(eId);
            Pool<Entity>.Release(entity);

            _updateEntities.Remove(eId);
            _lateUpdateEntities.Remove(eId);
            _fixedUpdateEntities.Remove(eId);
        }
    }

    public T AddComp<T>(ulong eId) where T : EntityComp, new()
    {
        if (_compPriority.TryGetValue(typeof(T), out var priority) &&
            _id2Entity.TryGetValue(eId, out var entity))
        {
            if (!entity.HasComp(priority))
            {
                var comp = new T();
                comp.Priority = priority;
                var type = typeof(T);

                var update = _updateType.Contains(type);
                var lateUpdate = _lateUpdateType.Contains(type);
                var fixedUpdate = _fixedUpdateType.Contains(type);
                if (update)
                {
                    _updateEntities.Add(eId);
                    _lateUpdateEntities.Add(eId);
                    _fixedUpdateEntities.Add(eId);
                }

                entity.AddComp(comp, update, lateUpdate, fixedUpdate);
            }
        }
        throw new ArgumentOutOfRangeException($"AddComp 异常");
    }
}