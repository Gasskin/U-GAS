using System;
using System.Collections.Generic;
using System.Linq;
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
        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("Key - 全英文")]
        [LabelWidth(80)]
        protected string key;

        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("备注")]
        [LabelWidth(80)]
        protected string backUp;

        private string DisplayName => $"{key} - {backUp}";

        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("计算模式")]
        [LabelWidth(80)]
        protected ECalculateMode eCalculateMode;

        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("最小值")]
        [LabelWidth(80)]
        protected float minValue = float.MinValue;

        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("最大值")]
        [LabelWidth(80)]
        protected float maxValue = float.MaxValue;

        protected EGameAttribute attributeType;

        private event Action<GameAttribute, float> OnPreCurrentValueChange;

        private event Action<GameAttribute, float, float> OnPostCurrentValueChange;

        // private event Func<BaseGameAttribute, float, float> OnPreBaseValueChange;
        private List<Func<GameAttribute, float, float>> _preBaseValueChangeListeners = new(32);
        private event Action<GameAttribute, float, float> OnPostBaseValueChange;

        public string Key => key;
        public string BackUp => backUp;
        public ECalculateMode ECalculateMode => eCalculateMode;
        public float MinValue => minValue;
        public float MaxValue => maxValue;
        public EGameAttribute AttributeType => attributeType;

        public float BaseValue { get; private set; }
        public float CurrentValue { get; private set; }

        public void SetCurrentValue(float value)
        {
            value = Mathf.Clamp(value, minValue, maxValue);
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

        public void SetMinValue(float min)
        {
            minValue = min;
        }

        public void SetMaxValue(float max)
        {
            maxValue = max;
        }


        public void RegisterPreBaseValueChange(Func<GameAttribute, float, float> func)
        {
            _preBaseValueChangeListeners.Add(func);
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
            _preBaseValueChangeListeners.Remove(func);
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
    }
}