using System;
using System.Collections.Generic;
using ProtoBuf;
using Sirenix.OdinInspector;
using UnityEngine;

namespace U_GAS
{
    [Serializable]
    [ProtoContract]
    public class GameAttributeSetUAsset : IUAsset
    {
        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("Key - 全英文")]
        [LabelWidth(80)]
        public string key;

        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("备注")]
        [LabelWidth(80)]
        public string backUp;

        private string DisplayName => $"{key} - {backUp}";

        [FoldoutGroup("$DisplayName")]
        [SerializeField]
        [LabelText("属性列表")]
        [LabelWidth(80)]
        [ProtoMember(1)]
        public List<EGameAttribute> attributes;
        
        public UAssetInfo GetUAssetInfo()
        {
            return new UAssetInfo()
            {
                selfKey = key,
                ownerType = nameof(GameAttributeSet),
                deSerializeField = "uAsset",
            };
        }
    }
}