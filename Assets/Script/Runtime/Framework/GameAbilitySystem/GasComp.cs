using System;
using cfg.Gas;
using UnityEngine;

[EntityComp(EntityCompPriority.c_Gas, true, false, false)]
public class GasComp : EntityComp
{
    public GameTagController GameTagController { get; private set; }

    public GameEffectController GameEffectController { get; private set; }

    public override void OnAdd()
    {
        GameTagController = new();
        GameTagController.Init(this);

        GameEffectController = new();
    }

    public override void Tick(float dt)
    {
    }

    public ulong ApplyGameEffectTo(int effectId, GasComp target)
    {
        var effect = SystemDriver.GetSystem<DesignSystem>().Tables.TbGameEffect.GetOrDefault(effectId);
        if (effect != null)
        {
            return ApplyGameEffectTo(effect, default, target);
        }
        return 0;
    }

    public ulong ApplyGameEffectTo(int effectId, GameEffectContext context, GasComp target)
    {
        var effect = SystemDriver.GetSystem<DesignSystem>().Tables.TbGameEffect.GetOrDefault(effectId);
        if (effect != null)
        {
            return ApplyGameEffectTo(effect, context, target);
        }
        return 0;
    }

    public ulong ApplyGameEffectTo(GameEffect effect, GasComp target)
    {
        return ApplyGameEffectTo(effect, default, target);
    }

    public ulong ApplyGameEffectTo(GameEffect effect, GameEffectContext context, GasComp target)
    {
        if (effect.Tags.Assets is not { Count: > 0 })
        {
            Debug.LogError($"GameEffect:{effect.Id}  assetTag不可以为空");
            return 0;
        }
        var spec = GameEffectSpec.Create(effect, this, target, context);
        spec.SetContext(context);
        return target.GameEffectController.AddGameEffectSpec(spec);
    }

    public void ApplyInstantGameEffect(GameEffectSpec spec)
    {
        foreach (var modifier in spec.GameEffect.modifiers)
        {
            var stat = GameAttributeComponent.GetAttribute(modifier.attribute);
            if (stat == null)
            {
                throw new NullReferenceException($"Stat of '{modifier.attribute}' not found!");
            }
            var magnitude = modifier.magnitude.CalculateMagnitude(spec);
            var newValue = stat.BaseValue;
            switch (modifier.operation)
            {
                case EModifierOperation.Add:
                    newValue += magnitude;
                    break;
                case EModifierOperation.Minus:
                    newValue -= magnitude;
                    break;
                case EModifierOperation.Multiply:
                    newValue *= magnitude;
                    break;
                case EModifierOperation.Divide:
                    newValue /= magnitude;
                    break;
                case EModifierOperation.Override:
                    newValue = magnitude;
                    break;
                default:
                    throw new NotSupportedException($"Operation '{modifier.operation}' not supported!");
            }
            stat.SetBaseValue(newValue, false);
        }
    }
}