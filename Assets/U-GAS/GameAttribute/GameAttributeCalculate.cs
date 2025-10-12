using System;
using System.Collections.Generic;
using UnityEngine;

namespace U_GAS
{
    /// <summary>
    /// 某个属性的数值计算器，根据modifier
    /// </summary>
    public class GameAttributeCalculate
    {
        private BaseGameAttribute _taget;
        private GameAbilityComponent _owner;
        private readonly List<string> _modifierCache = new();
        
        public GameAttributeCalculate(BaseGameAttribute attribute, GameAbilityComponent owner)
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
        
        
        private void UpdateCurrentValueWhenBaseValueIsDirty(BaseGameAttribute attribute, float oldBaseValue, float newBaseValue)
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