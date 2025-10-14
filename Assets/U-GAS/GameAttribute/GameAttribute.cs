using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace U_GAS
{
    public enum ECalculateMode
    {
        [LabelText("所有修饰器叠加")]
        Stacking,

        [LabelText("取最小修饰器")]
        MinValueOnly,

        [LabelText("取最大修饰器")]
        MaxValueOnly,
    }

    [Serializable]
    public class GameAttribute
    {
        protected GameAttributeUAsset uAsset;
        
        private event Action<GameAttribute, float> OnPreCurrentValueChange;

        private event Action<GameAttribute, float, float> OnPostCurrentValueChange;

        // private event Func<BaseGameAttribute, float, float> OnPreBaseValueChange;
        private List<Func<GameAttribute, float, float>> preBaseValueChangeListeners = new(32);
        private event Action<GameAttribute, float, float> OnPostBaseValueChange;

        public ECalculateMode ECalculateMode => uAsset.eCalculateMode;
        public float MinValue => uAsset.minValue;
        public float MaxValue => uAsset.maxValue;

        public float BaseValue { get; private set; }
        public float CurrentValue { get; private set; }

        public void SetCurrentValue(float value)
        {
            value = Mathf.Clamp(value, MinValue, MaxValue);
            OnPreCurrentValueChange?.Invoke(this, value);
            var oldValue = CurrentValue;
            CurrentValue = value;
            if (!Mathf.Approximately(oldValue, value))
            {
                OnPostCurrentValueChange?.Invoke(this, oldValue, value);
            }
        }

        public void SetBaseValue(float value)
        {
            if (preBaseValueChangeListeners != null)
            {
                foreach (var t in preBaseValueChangeListeners)
                    value = t.Invoke(this, value);
            }
            var oldValue = BaseValue;
            BaseValue = value;
            if (!Mathf.Approximately(oldValue, value))
            {
                OnPostBaseValueChange?.Invoke(this, oldValue, value);
            }
        }

        public void RegisterPreBaseValueChange(Func<GameAttribute, float, float> func)
        {
            preBaseValueChangeListeners.Add(func);
        }

        public void RegisterPostBaseValueChange(Action<GameAttribute, float, float> action)
        {
            OnPostBaseValueChange += action;
        }

        public void RegisterPreCurrentValueChange(Action<GameAttribute, float> action)
        {
            OnPreCurrentValueChange += action;
        }

        public void RegisterPostCurrentValueChange(Action<GameAttribute, float, float> action)
        {
            OnPostCurrentValueChange += action;
        }

        public void UnregisterPreBaseValueChange(Func<GameAttribute, float, float> func)
        {
            preBaseValueChangeListeners.Remove(func);
        }

        public void UnregisterPostBaseValueChange(Action<GameAttribute, float, float> action)
        {
            OnPostBaseValueChange -= action;
        }

        public void UnregisterPreCurrentValueChange(Action<GameAttribute, float> action)
        {
            OnPreCurrentValueChange -= action;
        }

        public void UnregisterPostCurrentValueChange(Action<GameAttribute, float, float> action)
        {
            OnPostCurrentValueChange -= action;
        }

        public string GetDeSerializeTarget()
        {
            return nameof(uAsset);
        }
    }
}