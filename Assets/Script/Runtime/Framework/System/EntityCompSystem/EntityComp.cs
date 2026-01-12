public abstract class EntityComp 
{
#region Priority
    public const int c_Gas = 1;
    public const int c_EntityView = 2;
#endregion
    
    public abstract int Priority { get; } 

    public Entity Entity;

    public bool IsValid => Entity is { Id: > 0 };
    
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
