using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

namespace Script.Runtime.Framework.System
{
    public class GameObjectPoolSystem : BaseSystem, ITickSystem
    {
        private class PooledGameObject : IPoolObject
        {
            public GameObject GameObject;
            public float LastUsedTime;

            public void OnRelease()
            {
                GameObject = null;
                LastUsedTime = 0f;
            }
        }

        private class GameObjectPool
        {
            public Queue<PooledGameObject> Pool = new();
            public int Capacity;
            public float ExpireTime;
        }

        private readonly Dictionary<string, GameObjectPool> _pools = new();
        private Transform _poolRoot;
        private int _defaultCapacity = 32;
        private float _defaultExpireTime = 64;

#if UNITY_EDITOR
        public Dictionary<string, int> GetCount = new();
        public Dictionary<string, int> ReleaseCount = new();
#endif

        public override async UniTask Initialize()
        {
            var go = new GameObject("[GameObjectPool]");
            Object.DontDestroyOnLoad(go);
            go.SetActive(false);
            _poolRoot = go.transform;
            await UniTask.Yield();
        }

        public override void Destroy()
        {
            ClearAll();
            if (_poolRoot != null)
            {
                Object.Destroy(_poolRoot.gameObject);
                _poolRoot = null;
            }
        }

        public void Tick(float dt)
        {
            foreach (var pair in _pools)
            {
                var data = pair.Value;
                if (data.ExpireTime <= 0)
                {
                    continue;
                }

                var pool = data.Pool;
                var count = pool.Count;
                for (int i = 0; i < count; i++)
                {
                    var pooledObj = pool.Dequeue();
                    pooledObj.LastUsedTime += dt;
                    if (pooledObj.LastUsedTime >= data.ExpireTime)
                    {
                        if (pooledObj.GameObject != null)
                        {
                            Object.Destroy(pooledObj.GameObject);
                        }
                        ObjectPool.ObjectPool.Release(pooledObj);
                    }
                    else
                    {
                        pool.Enqueue(pooledObj);
                    }
                }
            }
        }

        public void SetPool(string assetPath, int capacity, float expireTime)
        {
            if (_pools.TryGetValue(assetPath, out var data))
            {
                data.Capacity = capacity;
                data.ExpireTime = expireTime;
            }
            else
            {
                _pools[assetPath] = new GameObjectPool
                {
                    Capacity = capacity,
                    ExpireTime = expireTime
                };
            }
        }

        public async UniTask<GameObject> GetAsync(string assetPath, Transform parent = null)
        {
            if (!_pools.TryGetValue(assetPath, out var pool))
            {
                pool = new GameObjectPool
                {
                    Capacity = _defaultCapacity,
                    ExpireTime = _defaultExpireTime
                };
                _pools[assetPath] = pool;
            }

#if UNITY_EDITOR
            GetCount.TryAdd(assetPath, 0);
            GetCount[assetPath]++;
#endif

            while (pool.Pool.Count > 0)
            {
                var pooledObj = pool.Pool.Dequeue();
                if (pooledObj.GameObject != null)
                {
                    pooledObj.GameObject.transform.SetParent(parent, false);
                    ObjectPool.ObjectPool.Release(pooledObj);
                    return pooledObj.GameObject;
                }
            }

            var go = await SystemDriver.YooSystem.InitializeGameObjectAsync(assetPath, parent);
            return go;
        }

        public void Release(string assetPath, GameObject go)
        {
            if (go == null)
            {
                return;
            }

            if (!_pools.TryGetValue(assetPath, out var data))
            {
                throw new NullReferenceException($"no pool: {assetPath}");
            }

#if UNITY_EDITOR
            ReleaseCount.TryAdd(assetPath, 0);
            ReleaseCount[assetPath]++;
#endif

            if (data.Pool.Count >= data.Capacity)
            {
                Object.Destroy(go);
                return;
            }

            go.transform.SetParent(_poolRoot, false);
            var pooledObj = ObjectPool.ObjectPool.Get<PooledGameObject>();
            pooledObj.GameObject = go;
            pooledObj.LastUsedTime = 0f;
            data.Pool.Enqueue(pooledObj);
        }


        public void Clear(string assetPath)
        {
            if (!_pools.TryGetValue(assetPath, out var data))
            {
                return;
            }

            while (data.Pool.Count > 0)
            {
                var obj = data.Pool.Dequeue();
                if (obj.GameObject != null)
                {
                    Object.Destroy(obj.GameObject);
                }
                ObjectPool.ObjectPool.Release(obj);
            }

            _pools.Remove(assetPath);

#if UNITY_EDITOR
            GetCount.Remove(assetPath);
            ReleaseCount.Remove(assetPath);
#endif
        }

        public void ClearAll()
        {
            var keys = _pools.Keys.ToList();
            foreach (var key in keys)
            {
                Clear(key);
            }
        }
    }
}