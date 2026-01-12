using System;
using System.Collections.Generic;
using cfg.Gas;
using UnityEngine;

public class GasComp : EntityComp
{
    public GameTagController GameTagController { get; private set; }

    public GameEffectController GameEffectController { get; private set; }
    public GameAttributeController GameAttributeController { get; private set; }

    public override int Priority => c_Gas;
    public override bool NeedUpdate => true;

    public override void OnAdd()
    {
        GameTagController = new();
        GameEffectController = new();
        GameAttributeController = new();
    }

    public override void Tick(float dt)
    {
        GameEffectController?.Tick(dt);
    }

    public void Init(Dictionary<EAttributeId, float> initAttr)
    {
        GameTagController.Init(this);
        GameEffectController.Init(this);
        GameAttributeController.Init(this, initAttr);
    }

#region ApplyGameEffect
    public ulong ApplyGameEffectTo(int effectId, GasComp target)
    {
        var effect = SystemDriver.Get<DesignSystem>().Tables.TbGameEffect.GetOrDefault(effectId);
        if (effect != null)
        {
            return ApplyGameEffectTo(effect, default, target);
        }
        return 0;
    }

    public ulong ApplyGameEffectTo(int effectId, GameEffectContext context, GasComp target)
    {
        var effect = SystemDriver.Get<DesignSystem>().Tables.TbGameEffect.GetOrDefault(effectId);
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
#endregion

    public void ApplyInstantGameEffect(GameEffectSpec spec)
    {
        foreach (var modifier in spec.GameEffect.Modifiers)
        {
            var attr = GameAttributeController.GetAttribute(modifier.TargetAttr);
            if (attr == null)
            {
                throw new NullReferenceException($"Attribute is null: {modifier.TargetAttr}");
            }
            var magnitude = modifier.Magnitude.Calculate(spec);
            var newValue = attr.BaseValue;
            switch (modifier.Operate)
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
                    throw new NotSupportedException($"Operation '{modifier.Operate}' not supported!");
            }
            attr.SetBaseValue(newValue, false);
        }
    }
}