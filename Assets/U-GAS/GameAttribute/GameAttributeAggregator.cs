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
            UPool<GameEffect.GameEffectSpec>.Release(Spec);
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
        private readonly List<ModifierMagnitude> _modifierCache = new();

        public GameAttributeAggregator(GameAttribute attribute, GameAbilityComponent owner)
        {
            _taget = attribute;
            _owner = owner;
        }

        public void OnStart()
        {
            _taget.RegisterPostBaseValueChange(UpdateCurrentValueWhenBaseValueIsDirty);
        }

        public void OnStop()
        {
            _taget.UnregisterPostBaseValueChange(UpdateCurrentValueWhenBaseValueIsDirty);
        }


        private void UpdateCurrentValueWhenBaseValueIsDirty(GameAttribute attribute, float oldBaseValue, float newBaseValue)
        {
            if (Mathf.Approximately(oldBaseValue, newBaseValue))
            {
                return;
            }

            float newValue = CalculateNewValue();

            _taget.SetCurrentValue(newValue);
        }


        private float CalculateNewValue()
        {
            switch (_taget.ECalculateMode)
            {
                case ECalculateMode.Stacking:
                    var newValue = _taget.BaseValue;
                    return 0;
                case ECalculateMode.MaxValueOnly:
                    return 0;
                case ECalculateMode.MinValueOnly:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}