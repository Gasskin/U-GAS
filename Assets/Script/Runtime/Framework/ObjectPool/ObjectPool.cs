using System;
using System.Collections.Generic;

namespace Script.Runtime.Framework.ObjectPool
{
    public interface IPoolObject
    {
        void OnRelease();
    }
    
    public static class ObjectPool
    {
        private static Dictionary<Type, List<IPoolObject>> _pools = new();

#if UNITY_EDITOR
        private static Dictionary<Type, int> _getCount = new();
        private static Dictionary<Type, int> _releaseCount = new();
#endif

        public static T Get<T>() where T : IPoolObject, new()
        {
            var type = typeof(T);
            if (!_pools.TryGetValue(type, out var pools))
            {
                pools = new List<IPoolObject>();
                _pools[type] = pools;
            }
#if UNITY_EDITOR
            _getCount.TryAdd(type, 0);
            _getCount[type]++;
#endif
            if (_pools.Count > 0)
            {
                var t = pools[_pools.Count - 1];
                pools.RemoveAt(_pools.Count - 1);
                return (T)t;
            }
            return new T();
        }

        public static void Release<T>(T o) where T : IPoolObject
        {
            var type = typeof(T);
            if (!_pools.TryGetValue(type, out var pools))
            {
                pools = new List<IPoolObject>();
                _pools[type] = pools;
            }
#if UNITY_EDITOR
            _releaseCount.TryAdd(type, 0);
            _releaseCount[type]++;
#endif
            o.OnRelease();
            pools.Add(o);
        }
    }
}