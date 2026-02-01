using System;
using System.Collections.Generic;
using cfg.Gas;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System.GameAttribute;
using Script.Runtime.Framework.System.GameEffect;
using Script.Runtime.Framework.System.GameTag;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    [EntityCompPriority(Priority_Gas)]
    public class GasComp : EntityComp
    {
        public int Level { get; private set; }

        public GameTagController GameTagController { get; private set; } = new();

        public GameEffectController GameEffectController { get; private set; }= new();
        public GameAttributeController GameAttributeController { get; private set; }= new();

        public override bool NeedTick => true;

        public static GasComp Get(int level, Dictionary<EAttributeId, float> inAttr)
        {
            var comp = ObjectPool.ObjectPool.Get<GasComp>();
            comp.Level = level;
            
            comp.GameTagController.Init(comp);
            comp.GameEffectController.Init(comp);
            comp.GameAttributeController.Init(comp, inAttr);
            
            return comp;
        }

        public override async UniTask Initialize()
        {
            await UniTask.Yield();
        }

        public override void OnTick(float dt)
        {
            GameTagController?.Tick(dt);
            GameEffectController?.Tick(dt);
        }

    #region ApplyGameEffect
        public ulong ApplyGameEffectTo(int effectId, GasComp target)
        {
            var effect = SystemDriver.ConfigSystem.Tables.TbGameEffect.GetOrDefault(effectId);
            if (effect != null)
            {
                return ApplyGameEffectTo(effect, default, target);
            }
            return 0;
        }

        public ulong ApplyGameEffectTo(int effectId, GameEffectContext context, GasComp target)
        {
            var effect = SystemDriver.ConfigSystem.Tables.TbGameEffect.GetOrDefault(effectId);
            if (effect != null)
            {
                return ApplyGameEffectTo(effect, context, target);
            }
            return 0;
        }

        public ulong ApplyGameEffectTo(cfg.Gas.GameEffect effect, GasComp target)
        {
            return ApplyGameEffectTo(effect, default, target);
        }

        public ulong ApplyGameEffectTo(cfg.Gas.GameEffect effect, GameEffectContext context, GasComp target)
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
                if (modifier.Magnitude.IsMetaMagnitude)
                {
                    continue;
                }
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
                attr.SetBaseValue(newValue);
            }
        }
    }
}