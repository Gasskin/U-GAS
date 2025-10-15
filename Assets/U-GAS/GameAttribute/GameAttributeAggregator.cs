using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace U_GAS
{
    public class GameEffectModifierSpec : IUPoolObject
    {
        public GameEffect.GameEffectSpec Spec { get; private set; }

        public GameEffectModifier Modifier { get; private set; }

        public void Init(GameEffect.GameEffectSpec arg1, GameEffectModifier arg2)
        {
            Spec = arg1;
            Modifier = arg2;
        }

        public void OnRelease()
        {
            Spec = null;
            Modifier = null;
        }
    }

    /// <summary>
    /// 某个属性的数值计算器，根据modifier
    /// </summary>
    public class GameAttributeAggregator
    {
        private GameAttribute _taget;
        private GameAbilityComponent _owner;
        private readonly List<GameEffectModifierSpec> _modifierCache = new();

        public GameAttributeAggregator(GameAttribute attribute, GameAbilityComponent owner)
        {
            _taget = attribute;
            _owner = owner;
        }

        public void OnStart()
        {
            _taget.OnPostBaseValueChange += ReCalculateOnPostBaseValueChange;
            _owner.GameEffectComponent.OnGameEffectContainerDirty += ReCalculateOnGameEffectContainerDirty;
        }

        public void OnStop()
        {
            _taget.OnPostBaseValueChange -= ReCalculateOnPostBaseValueChange;
            _owner.GameEffectComponent.OnGameEffectContainerDirty -= ReCalculateOnGameEffectContainerDirty;
            
        }

        private void ReCalculateOnGameEffectContainerDirty()
        {
            var isDirty = _modifierCache.Count > 0;
            ReleaseModifierSpecCache();

            var specList = ListPool<GameEffect.GameEffectSpec>.Get();
            _owner.GameEffectComponent.GetSpecList(specList);
            foreach (var spec in specList)
            {
                if (spec.IsActive)
                {
                    foreach (var modifier in spec.GameEffect.modifiers)
                    {
                        if (modifier.attribute == _taget.Attribute)
                        {
                            var modifierSpec = UPool<GameEffectModifierSpec>.Get();
                            modifierSpec.Init(spec, modifier);
                            _modifierCache.Add(modifierSpec);
                        }
                    }
                }
            }

            isDirty = isDirty || _modifierCache.Count > 0;
            
            if (isDirty)
            {
                _taget.SetCurrentValue(CalculateNewValue());
            }
        }


        private void ReCalculateOnPostBaseValueChange(GameAttribute attribute, float oldBaseValue, float newBaseValue)
        {
            if (Mathf.Approximately(oldBaseValue, newBaseValue))
            {
                return;
            }

            _taget.SetCurrentValue(CalculateNewValue());
        }

        private void ReleaseModifierSpecCache()
        {
            foreach (var spec in _modifierCache)
            {
                UPool<GameEffectModifierSpec>.Release(spec);
            }
            _modifierCache.Clear();
        }


        private float CalculateNewValue()
        {
            switch (_taget.CalculateMode)
            {
                case ECalculateMode.Stacking:
                {
                    var newValue = _taget.BaseValue;
                    foreach (var modifierSpec in _modifierCache)
                    {
                        var spec = modifierSpec.Spec;
                        var modifier = modifierSpec.Modifier;
                        var magnitude = modifier.magnitude.CalculateMagnitude(spec, modifier.input);
                   
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
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    return newValue;
                }
                case ECalculateMode.MaxValueOnly:
                {
                    var hasOverride = false;
                    var max = float.MinValue;
                    foreach (var modifierSpec in _modifierCache)
                    {
                        var spec = modifierSpec.Spec;
                        var modifier = modifierSpec.Modifier;

                        if (modifier.operation != EModifierOperation.Override)
                        {
                            throw new InvalidOperationException("only support override operation.");
                        }

                        var magnitude = modifier.magnitude.CalculateMagnitude(spec, modifier.input);
                        max = Mathf.Max(max, magnitude);
                        hasOverride = true;
                    }

                    return hasOverride ? max : _taget.BaseValue;
                }
                case ECalculateMode.MinValueOnly:
                {
                    var hasOverride = false;
                    var min = float.MaxValue;
                    foreach (var modifierSpec in _modifierCache)
                    {
                        var spec = modifierSpec.Spec;
                        var modifier = modifierSpec.Modifier;

                        if (modifier.operation != EModifierOperation.Override)
                        {
                            throw new InvalidOperationException("only support override operation.");
                        }

                        var magnitude = modifier.magnitude.CalculateMagnitude(spec, modifier.input);
                        min = Mathf.Min(min, magnitude);
                        hasOverride = true;
                    }
                    return hasOverride ? min : _taget.BaseValue;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}