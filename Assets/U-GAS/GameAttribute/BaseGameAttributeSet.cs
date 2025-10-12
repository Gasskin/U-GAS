using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace U_GAS
{
    [Serializable]
    public class BaseGameAttributeSet
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

        public Dictionary<EGameAttribute,BaseGameAttribute> attributes;

        
        public string Key => key;
        public string BackUp => backUp;
        
        
        
#if UNITY_EDITOR
        [FoldoutGroup("$DisplayName")]
        [LabelText("属性列表")]
        [SerializeField]
        public List<EGameAttribute> attributeSelectors;
#endif
    }
}