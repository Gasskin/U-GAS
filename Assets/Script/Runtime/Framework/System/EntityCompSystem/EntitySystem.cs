using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;

public class EntitySystem : BaseSystem, ITickSystem, IFixedTickSystem, ILateTickSystem
{
    private Dictionary<ulong, Entity> _id2Entity = new();
    private List<Entity> _entities = new();
    private List<Entity> _updateEntities = new();
    private List<Entity> _lateUpdateEntities = new();
    private List<Entity> _fixedUpdateEntities = new();

    private ulong _entityIdGenerator = 1;

    public override async UniTask Initialize()
    {
        await UniTask.Yield();
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
        for (int i = 0; i < _updateEntities.Count; i++)
        {
            _updateEntities[i]?.Tick(dt);
        }
    }

    public void LateTick(float dt)
    {
        for (int i = 0; i < _lateUpdateEntities.Count; i++)
        {
            _lateUpdateEntities[i]?.Tick(dt);
        }
    }

    public void FixedTick(float dt)
    {
        for (int i = 0; i < _fixedUpdateEntities.Count; i++)
        {
            _fixedUpdateEntities[i]?.Tick(dt);
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
            if (entity.NeedUpdate)
            {
                _updateEntities.RemoveAt(entity.UpdateIndex);
            }
            if (entity.NeedLateUpdate)
            {
                _lateUpdateEntities.RemoveAt(entity.LateUpdateIndex);
            }
            if (entity.NeedFixedUpdate)
            {
                _fixedUpdateEntities.RemoveAt(entity.FixedUpdateIndex);
            }

            Pool<Entity>.Release(entity);
        }
    }

    public void Search<T>(int priority, List<T> comps) where T : EntityComp, new()
    {
        comps.Clear();
        if (priority < 0)
        {
            return;
        }
        foreach (var e in _entities)
        {
            if (e.HasComp(priority, out var comp))
            {
                comps.Add((T)comp);
            }
        }
    }

    public void RegisterUpdate(Entity entity)
    {
        _updateEntities.Add(entity);
        entity.UpdateIndex = _updateEntities.Count - 1;
    }

    public void RegisterLateUpdate(Entity entity)
    {
        _lateUpdateEntities.Add(entity);
        entity.LateUpdateIndex = _lateUpdateEntities.Count - 1;
    }

    public void RegisterFixedUpdate(Entity entity)
    {
        _fixedUpdateEntities.Add(entity);
        entity.FixedUpdateIndex = _fixedUpdateEntities.Count - 1;
    }
}