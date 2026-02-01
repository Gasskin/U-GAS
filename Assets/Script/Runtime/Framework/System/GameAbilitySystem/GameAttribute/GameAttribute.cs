using System;
using System.Collections.Generic;
using cfg.Gas;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System.GameEffect;
using UnityEngine;
using UnityEngine.Pool;

namespace Script.Runtime.Framework.System.GameAttribute
{
    public class AttrModifier : IPoolObject
    {
        public GameEffectSpec Spec;
        public Modifier Modifier;

        public void OnRelease()
        {
            Spec = null;
            Modifier = null;
        }
    }

    public class GameAttribute
    {
    #region Static
        private static readonly Dictionary<EAttributeId, EAttributeId> _attrMax = new()
        {
            // 生命值
            { EAttributeId.HpNow, EAttributeId.HpMax },
            // 抗性
            { EAttributeId.FireArmorNow, EAttributeId.FireArmorMax },
            { EAttributeId.IceArmorNow, EAttributeId.FireArmorMax },
            { EAttributeId.LightArmorNow, EAttributeId.FireArmorMax },
            { EAttributeId.ChaosArmorNow, EAttributeId.FireArmorMax },
        };

        private static readonly Dictionary<EAttributeId, float> _attrMaxStatic = new()
        {
        };

        private static readonly Dictionary<EAttributeId, float> _attrMinStatic = new()
        {
            // 抗性
            { EAttributeId.FireArmorNow, float.MinValue },
            { EAttributeId.IceArmorNow, float.MinValue },
            { EAttributeId.LightArmorNow, float.MinValue },
            { EAttributeId.ChaosArmorNow, float.MinValue },
        };
    #endregion

        public EAttributeId AttributeId { get; private set; }

        public float BaseValue { get; private set; }

        public float CurrentValue { get; private set; }

        public event Func<float, float> OnPreBaseValueChange
        {
            add => _onPreBaseValueChange.Add(value);
            remove => _onPreBaseValueChange.Remove(value);
        }

        private List<Func<float, float>> _onPreBaseValueChange = new();

        // public event Action<float, float> OnPostBaseValueChange;

        public event Action<float, float> OnPostCurrentValueChange;

        private List<AttrModifier> _modifierSpecs = new();
        private GasComp _owner;

        public GameAttribute(GasComp owner, EAttributeId id)
        {
            AttributeId = id;
            BaseValue = 0;
            CurrentValue = 0;
            _owner = owner;
        }

        public void InitValue(float value)
        {
            BaseValue = value;
            CurrentValue = value;
        }

        public void OnGameEffectDirty()
        {
            var isDirty = _modifierSpecs.Count > 0;

            ReleaseModifiers();

            var effects = ListPool<GameEffectSpec>.Get();
            _owner.GameEffectController.GetGameEffectSpecs(effects);
            foreach (var spec in effects)
            {
                if (spec.IsActive)
                {
                    foreach (var modifier in spec.GameEffect.Modifiers)
                    {
                        if (modifier.TargetAttr == AttributeId)
                        {
                            TryAddRelativeAttrListener(modifier.Magnitude);
                            var cache = ObjectPool.ObjectPool.Get<AttrModifier>();
                            cache.Modifier = modifier;
                            cache.Spec = spec;
                            _modifierSpecs.Add(cache);
                        }
                    }
                }
            }
            ListPool<GameEffectSpec>.Release(effects);

            isDirty = isDirty || _modifierSpecs.Count > 0;

            if (isDirty)
            {
                CalculateCurrent();
            }
        }


        public void SetBaseValue(float newValue)
        {
            foreach (var func in _onPreBaseValueChange)
            {
                newValue = func(newValue);
            }
            newValue = ClampAttribute(newValue);
            var oldValue = BaseValue;
            BaseValue = newValue;
            if (!Mathf.Approximately(oldValue, newValue))
            {
                CalculateCurrent();
            }
        }

        private void SetCurrentValue(float newValue)
        {
            newValue = ClampAttribute(newValue);
            var oldValue = CurrentValue;
            CurrentValue = newValue;
            if (!Mathf.Approximately(oldValue, newValue))
            {
                OnPostCurrentValueChange?.Invoke(oldValue, newValue);
            }
        }

        private float ClampAttribute(float input)
        {
            if (_attrMaxStatic.TryGetValue(AttributeId, out var max))
            {
            }
            else if (_attrMax.TryGetValue(AttributeId, out var maxId))
            {
                max = _owner.GameAttributeController.GetCurrentValue(maxId);
            }
            else
            {
                max = float.MaxValue;
            }

            if (_attrMinStatic.TryGetValue(AttributeId, out var min))
            {
            }
            else
            {
                min = 0f;
            }

            input = Mathf.Clamp(input, min, max);
            return input;
        }

        private void CalculateCurrent()
        {
            var newValue = BaseValue;
            foreach (var mod in _modifierSpecs)
            {
                if (mod.Modifier.Magnitude.IsMetaMagnitude)
                {
                    Debug.LogError("MetaMagnitude 在此处不合法");
                    continue;
                }
                var magnitude = mod.Modifier.Magnitude.Calculate(mod.Spec);

                switch (mod.Modifier.Operate)
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
                        throw new ArgumentOutOfRangeException();
                }
            }

            SetCurrentValue(newValue);
        }

        private void ReleaseModifiers()
        {
            foreach (var m in _modifierSpecs)
            {
                TryRemoveRelativeAttrListener(m.Modifier.Magnitude);
                ObjectPool.ObjectPool.Release(m);
            }
            _modifierSpecs.Clear();
        }


        private void TryAddRelativeAttrListener(BaseMagnitude magnitude)
        {
            if (magnitude is not RelativeAttrMagnitude m)
            {
                return;
            }
            var v = _owner.GameAttributeController.GetAttribute(m.RelativeAttrBase);
            var k = _owner.GameAttributeController.GetAttribute(m.RelativeAttrK);
            var b = _owner.GameAttributeController.GetAttribute(m.RelativeAttrB);
            v.OnPostCurrentValueChange += OnPostDependCurrentValueChange;
            k.OnPostCurrentValueChange += OnPostDependCurrentValueChange;
            b.OnPostCurrentValueChange += OnPostDependCurrentValueChange;
        }


        private void TryRemoveRelativeAttrListener(BaseMagnitude magnitude)
        {
            if (magnitude is not RelativeAttrMagnitude m)
            {
                return;
            }
            var v = _owner.GameAttributeController.GetAttribute(m.RelativeAttrBase);
            var k = _owner.GameAttributeController.GetAttribute(m.RelativeAttrK);
            var b = _owner.GameAttributeController.GetAttribute(m.RelativeAttrB);
            v.OnPostCurrentValueChange -= OnPostDependCurrentValueChange;
            k.OnPostCurrentValueChange -= OnPostDependCurrentValueChange;
            b.OnPostCurrentValueChange -= OnPostDependCurrentValueChange;
        }


        private void OnPostDependCurrentValueChange(float oV, float nV)
        {
            CalculateCurrent();
        }
    }
}