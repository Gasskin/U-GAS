public abstract class BaseSystem
{
    public abstract void Initialize();
    public abstract void Close();
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