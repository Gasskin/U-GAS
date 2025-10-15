using UnityEngine.Pool;

namespace U_GAS
{
    public interface IUPoolObject
    {
        void OnRelease();
    }

    public static class UPool<T> where T : class, IUPoolObject, new()
    {
        private static IObjectPool<T> _pool = new ObjectPool<T>(() => new T(), null, (t) => t.OnRelease());


        public static T Get()
        {
            return _pool.Get();
        }

        public static void Release(T instance)
        {
            _pool.Release(instance);
        }

        public static void Clear()
        {
            _pool.Clear();
        }
    }
}