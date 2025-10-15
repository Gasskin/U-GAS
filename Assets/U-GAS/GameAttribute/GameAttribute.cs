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
        public event Action<GameAttribute, float> OnPreCurrentValueChange;

        public event Action<GameAttribute, float, float> OnPostCurrentValueChange;

        private List<Func<GameAttribute, float, float>> _preBaseValueChangeListeners = new(32);
        
        public event Func<GameAttribute, float, float> PreBaseValueChangeListeners
        {
            add => _preBaseValueChangeListeners.Add(value);
            remove => _preBaseValueChangeListeners.Remove(value);
        }
        public event Action<GameAttribute, float, float> OnPostBaseValueChange;

        public ECalculateMode CalculateMode { get; private set; }
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public float BaseValue { get; private set; }
        public float CurrentValue { get; private set; }

        public EGameAttribute Attribute { get; private set; }


        public GameAttribute(EGameAttribute attribute,ECalculateMode calculateMode, float initValue, float min, float max)
        {
            BaseValue = initValue;
            CalculateMode = calculateMode;
            CurrentValue = initValue;
            MinValue = min;
            MaxValue = max;
            Attribute = attribute;
        }

        public void InitValue(float value)
        {
            BaseValue = value;
            CurrentValue = BaseValue;
        }

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
            if (_preBaseValueChangeListeners != null)
            {
                foreach (var t in _preBaseValueChangeListeners)
                    value = t.Invoke(this, value);
            }
            var oldValue = BaseValue;
            BaseValue = value;
            if (!Mathf.Approximately(oldValue, value))
            {
                OnPostBaseValueChange?.Invoke(this, oldValue, value);
            }
        }
    }
}