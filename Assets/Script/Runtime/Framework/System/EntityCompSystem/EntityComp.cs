public abstract class EntityComp 
{
    public abstract int Priority { get; } 

    public Entity Entity;
    
    public virtual bool NeedUpdate { get; } = false;
    public virtual bool NeedLateUpdate { get; } = false;
    public virtual bool NeedFixedUpdate { get; } = false;
    
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

/// <summary>
/// 要求>0
/// </summary>
public class EntityCompPriority
{
    public const int c_Gas = 1;
}