using cfg.Gas;
using UnityEngine.Pool;

public struct GameEffectContext
{
}

public class GameEffectSpec : IPoolObject
{
#region static
    private static ulong s_idFactory;

    public static GameEffectSpec Create(GameEffect ge, GasComp source, GasComp target, GameEffectContext context)
    {
        var spec = Pool<GameEffectSpec>.Get();
        if (s_idFactory >= ulong.MaxValue)
        {
            s_idFactory = 0;
        }
        spec.Id = ++s_idFactory;
        spec.GameEffect = ge;
        spec.Source = source;
        spec.Target = target;
        spec.Context = context;
        spec.IsActive = false;
        spec.Source = source;
        spec.Target = target;
        spec.ActiveTime = 0;
        spec.ElapsedTime = 0;
        spec.StackCount = 1;
        if (ge.Duration.FirstPeriodImmediately)
        {
            spec.PeriodRemaining = 0;
        }
        else
        {
            spec.PeriodRemaining = ge.Duration.Period;
        }
        if (ge.Duration.DurationType != EGameEffectDurationType.Instant &&
            ge.Duration.Period >= 0.01f &&
            ge.Duration.PeriodGe_Ref != null)
        {
            spec._periodGameEffectSpec = Create(ge.Duration.PeriodGe_Ref, source, target, context);
        }
        
        return spec;
    }
#endregion
    public bool IsValid => Id > 0;
    public ulong Id { get; private set; }

    public GameEffect GameEffect { get; private set; }
    public GameEffectContext Context { get; private set; }
    public GasComp Source { get; private set; }
    public GasComp Target { get; private set; }

    public float ActiveTime { get; private set; }

    public float ElapsedTime { get; private set; }

    public int StackCount { get; private set; }

    public float PeriodRemaining { get; private set; }

    public bool IsActive { get; private set; }


    private GameEffectSpec _periodGameEffectSpec;

    public void OnRelease()
    {
        Id = 0;
        GameEffect = null;
        Context = default;
        Source = null;
        Target = null;
        ActiveTime = 0;
        ElapsedTime = 0;
        StackCount = 0;
        PeriodRemaining = 0;
        IsActive = false;
        if (_periodGameEffectSpec != null) 
        {
            Pool<GameEffectSpec>.Release(_periodGameEffectSpec);
        }
        _periodGameEffectSpec = null;
    }

#region 条件判断
    public bool CanApply()
    {
        if (!IsValid)
        {
            return false;
        }
        return Target.GameTagController.HasAllTag(GameEffect.ApplyRequiredTagContainer.Tags);
    }
    
    public bool CanRunning()
    {
        if (!IsValid)
        {
            return false;
        }
        return Target.GameTagController.HasAllTag(GameEffect.OnGoingRequiredTagContainer.Tags);
    }
    
    public bool IsImmune()
    {
        if (!IsValid)
        {
            return true;
        }
        // 判断自身是否被免疫
        if (Target.GameTagController.HasAnyTag(GameEffect.ImmuneWhenTagContainer.Tags))
        {
            return true;
        }
        // todo 判断是否被其他GE阻挡
        // var flag = false;
        // var list = ListPool<GameEffectSpec>.Get();
        // Target.GameTagController.GetGameEffectSpecs(list);
        // for (int i = 0; i < list.Count; i++)
        // {
        //     var spec = list[i];
        //     if (GameEffect.assetTags != null
        //         && spec.GameEffect.BlockGameWithTagsContainer.HasAnyTags(GameEffect.assetTags))
        //     {
        //         flag = true;
        //         break;
        //     }
        //     if (GameEffect.grantedTags != null
        //         && spec.GameEffect.BlockGameWithTagsContainer.HasAnyTags(GameEffect.grantedTags))
        //     {
        //         flag = true;
        //         break;
        //     }
        // }
        // ListPool<GameEffectSpec>.Release(list);
        // return flag;
        return true;
    }
#endregion

#region 生命周期
    public void OnExecute()
    {
        Target.ApplyInstantGameEffect(this);
        if (IsValid)
        {
            // ge是可能导致自身被移除
            // 比如当一个GE被移除时，会移除他的period ge
            Target.GameEffectController.RemoveGameEffectWithAnyTags(GameEffect.RemoveGeWithTagContainer);
        }
    }
#endregion
    
    public void SetContext(GameEffectContext context)
    {
        Context = context;
    }

}