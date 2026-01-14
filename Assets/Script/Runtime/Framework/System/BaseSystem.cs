using Cysharp.Threading.Tasks;

public abstract class BaseSystem
{
    public bool Initialized = false;
    public abstract UniTask Initialize();
    public abstract void Destroy();
}

public interface ITickSystem
{
    public void Tick(float dt);
}

public interface ILateTickSystem
{
    public void LateTick(float dt);
}

public interface IFixedTickSystem
{
    public void FixedTick(float dt);
}