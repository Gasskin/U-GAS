using UnityEngine.Pool;

namespace Script.Runtime.Framework.ObjectPool
{
    public interface IPoolObject
    {
        void OnRelease();
    }

    public static class Pool<T> where T : class, IPoolObject, new()
    {
        private static ObjectPool<T> s_pool = new((() => new T()));

        public static T Get()
        {
            return s_pool.Get();
        }

        public static void Release(T t)
        {
            t.OnRelease();
            s_pool.Release(t);
        }
    }
}