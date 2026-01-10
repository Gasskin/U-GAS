using UnityEngine.Pool;

public interface IPoolObject
{
    void OnRelease();
}

public static class Pool<T> where T : class, IPoolObject
{
    private static ObjectPool<T> s_pool = new(null);

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