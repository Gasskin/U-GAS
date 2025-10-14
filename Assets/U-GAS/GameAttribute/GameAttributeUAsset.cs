using System;
using ProtoBuf;
using Sirenix.OdinInspector;
using UnityEngine;

namespace U_GAS
{
    [ProtoContract]
    [Serializable]
    public class GameAttributeUAsset : IUAsset
    {
#if UNITY_EDITOR
        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("Key - 全英文")]
        [LabelWidth(80)]
        private string key;

        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("备注")]
        [LabelWidth(80)]
        private string backUp;
        
        private string DisplayName => $"{key} - {backUp}";
        
        public string Key => key;
        public string BackUp => backUp;
#endif
        
        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("计算模式")]
        [LabelWidth(80)]
        [ProtoMember(1)]
        public ECalculateMode eCalculateMode;

        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("最小值")]
        [LabelWidth(80)]
        [ProtoMember(2)]
        
        public float minValue = float.MinValue;
        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("最大值")]
        [LabelWidth(80)]
        [ProtoMember(3)]
        public float maxValue = float.MaxValue;

        public UAssetInfo GetUAssetInfo()
        {
            return new UAssetInfo()
            {
                selfKey = key,
                ownerType = nameof(GameAttribute),
                deSerializeField = "uAsset",
            };
        }
    }
}