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
        [LabelText("Key - 全英文")]
        [LabelWidth(80)]
        public string key;

        [FoldoutGroup("$DisplayName")]
        [LabelText("备注")]
        [LabelWidth(80)]
        public string backUp;

        private string DisplayName => $"{key} - {backUp}";
#endif

        [FoldoutGroup("$DisplayName")]
        [LabelText("计算模式")]
        [LabelWidth(80)]
        [ProtoMember(1)]
        public ECalculateMode eCalculateMode;

        [FoldoutGroup("$DisplayName")]
        [LabelText("最小值")]
        [LabelWidth(80)]
        [ProtoMember(2)]
        public float minValue;

        [FoldoutGroup("$DisplayName")]
        [LabelText("最大值")]
        [LabelWidth(80)]
        [ProtoMember(3)]
        public float maxValue;

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