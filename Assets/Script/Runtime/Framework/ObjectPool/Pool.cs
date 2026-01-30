using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Script.Runtime.Framework.ObjectPool
{
    public interface IPoolObject
    {
        void OnRelease();
    }

    public static class Pool<T> where T : class, IPoolObject, new()
    {
        private static readonly ObjectPool<T> _pool = new((() => new T()));
        
        public static T Get()
        {
            return _pool.Get();
        }

        public static void Release(T t)
        {
            t.OnRelease();
            _pool.Release(t);
        }
    }
}