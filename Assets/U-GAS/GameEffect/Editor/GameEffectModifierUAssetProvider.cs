using System;
using Sirenix.OdinInspector;

namespace U_GAS.Editor
{
    [Serializable]
    public class GameEffectModifierUAssetProvider : IUAssetProvider
    {
        [LabelText("目标属性")]
        [LabelWidth(80)]
        public EGameAttribute attribute;

        [LabelText("操作类型")]
        [LabelWidth(80)]
        public EModifierOperation operation;
        
        [LabelText("输入值")]
        [LabelWidth(80)]
        public float input;

        [LabelText("规格计算器")]
        [LabelWidth(80)]
        public ModifierMagnitudeUAssetProvider magnitude;

        public IUAsset GetUAsset()
        {
            var asset = new GameEffectModifier();
            asset.attribute = attribute;
            asset.operation = operation;
            asset.input = input;
            if (magnitude is IUAssetProvider provider)
            {
                asset.magnitude = (ModifierMagnitude)provider.GetUAsset();
            }
            return asset;
        }
    }
}