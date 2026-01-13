using Cysharp.Threading.Tasks;

public abstract class EntityComp
{
#region Priority
    // >0 是为了保险
    public const int GAS = 1;
    public const int VIEW = 2;
    public const int SKILL_SPELL = 3;
    public const int STATE_MACHINE = 4;
#endregion

    public abstract int Priority { get; }

    public Entity Entity;

    public bool IsValid => Entity is { Id: > 0 };

    public virtual bool NeedTick { get; } = false;
    public virtual bool NeedLateTick { get; } = false;
    public virtual bool NeedFixedTick { get; } = false;

    public virtual async UniTask Initialize()
    {
        await UniTask.Yield();
    }

    public virtual void Destroy()
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