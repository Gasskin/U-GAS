using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class EntitySystem : BaseSystem, ITickSystem, IFixedTickSystem, ILateTickSystem
{
    private Dictionary<ulong, Entity> _id2Entity = new();
    private List<Entity> _entities = new();
    private List<Entity> _tickEntities = new();
    private List<Entity> _lateTickEntities = new();
    private List<Entity> _fixedTickEntities = new();

    private ulong _entityIdGenerator = 1;

    public override async UniTask Initialize()
    {
        await UniTask.Yield();
    }

    public override void Destroy()
    {
        var entities = _id2Entity.Keys.ToList();
        foreach (var id in entities)
        {
            DestroyEntity(id);
        }
    }

    public void Tick(float dt)
    {
        for (int i = 0; i < _tickEntities.Count; i++)
        {
            _tickEntities[i]?.Tick(dt);
        }
    }

    public void LateTick(float dt)
    {
        for (int i = 0; i < _lateTickEntities.Count; i++)
        {
            _lateTickEntities[i]?.LateTick(dt);
        }
    }

    public void FixedTick(float dt)
    {
        for (int i = 0; i < _fixedTickEntities.Count; i++)
        {
            _fixedTickEntities[i]?.FixedTick(dt);
        }
    }

    public Entity CreateEntity()
    {
        if (_entityIdGenerator >= ulong.MaxValue)
        {
            _entityIdGenerator = 1;
        }
        var id = _entityIdGenerator++;
        if (_id2Entity.ContainsKey(id))
        {
            throw new ArgumentOutOfRangeException($"EntityId 重复");
        }
        var e = Pool<Entity>.Get();
        e.Init(id, this);
        _id2Entity[id] = e;
        _entities.Add(e);
        e.Index = _entities.Count - 1;
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

            _entities.RemoveAt(entity.Index);
            if (entity.TickIndex >= 0)
            {
                _tickEntities.RemoveAt(entity.TickIndex);
            }
            if (entity.LateTickIndex >= 0)
            {
                _lateTickEntities.RemoveAt(entity.LateTickIndex);
            }
            if (entity.FixedTickIndex >= 0)
            {
                _fixedTickEntities.RemoveAt(entity.FixedTickIndex);
            }

            Pool<Entity>.Release(entity);
        }
    }

    public void Search<T>(int priority, List<T> comps) where T : EntityComp
    {
        comps.Clear();
        if (priority < 0)
        {
            return;
        }
        for (int i = 0; i < _entities.Count; i++)
        {
            if (_entities[i].HasComp(priority, out T comp))
            {
                comps.Add(comp);
            }
        }
    }

    public void RegisterUpdate(Entity entity)
    {
        _tickEntities.Add(entity);
        entity.TickIndex = _tickEntities.Count - 1;
    }

    public void RegisterLateUpdate(Entity entity)
    {
        _lateTickEntities.Add(entity);
        entity.LateTickIndex = _lateTickEntities.Count - 1;
    }

    public void RegisterFixedUpdate(Entity entity)
    {
        _fixedTickEntities.Add(entity);
        entity.FixedTickIndex = _fixedTickEntities.Count - 1;
    }
}