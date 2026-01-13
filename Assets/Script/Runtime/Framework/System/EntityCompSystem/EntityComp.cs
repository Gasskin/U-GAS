public abstract class EntityComp
{
#region Priority
    public const int c_Gas = 0;
    public const int c_View = 1;
    public const int c_SkillSpell = 2;
#endregion

    public abstract int Priority { get; }

    public Entity Entity;

    public bool IsValid => Entity is { Id: > 0 };

    public virtual bool NeedTick { get; } = false;
    public virtual bool NeedLateTick { get; } = false;
    public virtual bool NeedFixedTick { get; } = false;

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