public abstract class EntityComp
{
    public int Priority;
    public Entity Entity;
    
    public virtual void OnAdd()
    {
    }

    public virtual void OnRemove()
    {
    }

    public virtual void Tick(float dt)
    {
    }

    public virtual void LateTick(float dt)
    {
    }

    public virtual void FixedTick(float dt)
    {
    }
}

public class EntityCompPriority
{
    public const int c_Gas = 0;
}