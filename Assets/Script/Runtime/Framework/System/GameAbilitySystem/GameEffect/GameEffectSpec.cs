using System;
using System.Collections.Generic;
using cfg.Gas;
using UnityEngine;
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

    private List<GameEffectSpec> _pool = new(32);

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
        // 判断是否被其他GE阻挡
        var flag = false;
        Target.GameEffectController.GetGameEffectSpecs(_pool);
        for (int i = 0; i < _pool.Count; i++)
        {
            var spec = _pool[i];
            if (spec.GameEffect.BlockGeWithTagContainer.HasAnyTags(GameEffect.AssetTagContainer.Tags))
            {
                flag = true;
                break;
            }
            if (spec.GameEffect.BlockGeWithTagContainer.HasAnyTags(GameEffect.GrantedTagContainer.Tags))
            {
                flag = true;
                break;
            }
        }
        return flag;
    }
#endregion

#region 生命周期
    public void OnExecute()
    {
        Target.ApplyInstantGameEffect(this);
        if (IsValid)
        {
            Target.GameEffectController.RemoveGameEffectWithAnyTags(GameEffect.RemoveGeWithTagContainer.Tags);
        }
    }

    public void OnAdd()
    {
        Target.GameAttributeController.OnGameEffectDirty();
        if (IsValid)
        {
     
        }
    }

    public void OnRemove()
    {
        Target.GameAttributeController.OnGameEffectDirty();
        if (IsValid)
        {
        }
    }

    public void OnActive()
    {
        if (IsActive)
        {
            return;
        }
        IsActive = true;
        ActiveTime = SystemDriver.GetSystem<BattleTimeSystem>().Now;
        
        Target.GameTagController.AddTagsWithDirty(GameEffect.GrantedTagContainer.Tags);

        if (IsValid)
        {
            Target.GameAttributeController.OnGameEffectDirty();
        }

        if (IsValid)
        {
            Target.GameEffectController.RemoveGameEffectWithAnyTags(GameEffect.RemoveGeWithTagContainer.Tags);
        }

        if (IsValid)
        {
            
        }
    }

    public void OnDeActive()
    {
        if (!IsActive)
        {
            return;
        }
        IsActive = false;
        Target.GameTagController.RemoveTagsWithDirty(GameEffect.GrantedTagContainer.Tags);

        if (IsValid)
        {
            Target.GameAttributeController.OnGameEffectDirty();
        }

        if (IsValid)
        {
            
        }
    }

    public void OnTick(float dt)
    {
        if (GameEffect.Duration.DurationType == EGameEffectDurationType.Instant)
        {
            return;
        }
        
        TickPeriod(dt);
        
        if (!IsValid)
        {
            return;
        }
        
        TickDuration();
        
        ElapsedTime += dt;
    }
#endregion

#region 堆叠
    public void AddStack(int count, bool refreshDuration, bool refreshPeriod)
    {
        if (count <= 0)
        {
            return;
        }

        var oldCount = StackCount;
        StackCount += count;

        if (GameEffect.Stack.CountLimit > 0)
        {
            StackCount = Math.Clamp(StackCount, 1, GameEffect.Stack.CountLimit);
        }

        // on stack change?
        // if stack change?

        if (refreshDuration)
        {
            RefreshDuration();
        }
        if (refreshPeriod)
        {
            PeriodRemaining = GameEffect.Duration.Period;
        }
    }

    public void RemoveStack(int count, bool refreshDuration)
    {
        if (count >= StackCount)
        {
            Target.GameEffectController.InternalRemoveGameEffectSpec(this);
        }
        else
        {
            var oldCount = StackCount;
            StackCount -= count;

            if (refreshDuration)
            {
                RefreshDuration();
            }
        }
    }
#endregion

#region 持续时间
    private void TickPeriod(float dt)
    {
        if (_periodGameEffectSpec == null || GameEffect.Duration.Period <= 0.01f)
        {
            return;
        }

        // 警告：如果配置的period间隔太小的话，这里会存在问题
        // 首帧保护，runningTime如果是一个极小值，可能会有浮点数精度问题，干脆第一帧不进行逻辑运算
        var runningTime = SystemDriver.GetSystem<BattleTimeSystem>().Now - ActiveTime;
        if (runningTime < Mathf.Epsilon)
        {
            return;
        }

        // 如果运行时间超过了duration，那么计算period时，我们只计算duration内的那一部分
        if (GameEffect.Duration.DurationType == EGameEffectDurationType.Duration)
        {
            var outDuration = runningTime - GameEffect.Duration.Duration;
            if (outDuration >= 0)
            {
                // 此时应该是最后一次执行
                dt -= outDuration;
                // 避免误差, 保证最后一次边界得到执行
                dt += 0.0001f;
                dt = Mathf.Max(dt, 0);
            }
        }

        PeriodRemaining -= dt;

        // period触发的行为可能导致所属的GE(_spec)失活/移除, 所属GE移除时会将_spec设置为null
        while (PeriodRemaining < 0f && IsValid && IsActive)
        {
            // 不能直接重置为Period, 累计误差
            PeriodRemaining += GameEffect.Duration.Period;
            _periodGameEffectSpec?.OnExecute();
        }
    }

    private void TickDuration()
    {
        if (GameEffect.Duration.DurationType != EGameEffectDurationType.Duration)
        {
            return;
        }

        var remaining = GameEffect.Duration.Duration - ElapsedTime;
        if (remaining > 0)
        {
            return;
        }

        if (GameEffect.Stack.StackType == EGameEffectStackType.None)
        {
            Target.GameEffectController.InternalRemoveGameEffectSpec(this);
        }
        else
        {
            switch (GameEffect.Stack.ExpireType)
            {
                case EGameEffectStackExpireType.RemoveAll:
                {
                    Target.GameEffectController.InternalRemoveGameEffectSpec(this);
                    break;
                }
                case EGameEffectStackExpireType.RemoveSingle:
                {
                    RemoveStack(1, true);
                    break;
                }
            }
        }
    }

    private void RefreshDuration()
    {
        ActiveTime = SystemDriver.GetSystem<BattleTimeSystem>().Now;
        ElapsedTime = 0.0f;
    }
#endregion

    public void SetContext(GameEffectContext context)
    {
        Context = context;
    }
}