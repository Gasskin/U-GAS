using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace U_GAS.Editor
{
    [Serializable]
    public class GameEffectStackUAssetProvider: IUAssetProvider
    {
        [LabelText("标识")]
        [Tooltip("标识这个GE的key，相同key的GE才可以堆叠")]
        [LabelWidth(80)]
        [HorizontalGroup]
        public string key;

        [Button("重置标识")]
        [HorizontalGroup(0.2f)]
        public void ResetKey()
        {
            if (Selection.activeObject != null) 
            {
                key = Selection.activeObject.name;
            }
        }
        
        [LabelText("堆叠类型")]
        [LabelWidth(80)]
        public EGameEffectStackPolicy stackPolicy;

        [LabelText("最大层数")]
        [HideIf("@stackPolicy == EGameEffectStackPolicy.None")]
        [LabelWidth(80)]
        public int maxCount;
        
        [LabelText("持续时间刷新")]
        [HideIf("@stackPolicy == EGameEffectStackPolicy.None")]
        [LabelWidth(80)]
        public EGameEffectStackDurationRefreshPolicy durationRefreshPolicy;
        
        [LabelText("生效周期重置")]
        [HideIf("@stackPolicy == EGameEffectStackPolicy.None")]
        [LabelWidth(80)]
        public EGameEffectStackPeriodResetPolicy periodResetPolicy;
        
        [LabelText("失效策略")]
        [HideIf("@stackPolicy == EGameEffectStackPolicy.None")]
        [LabelWidth(80)]
        public EGameEffectStackExpirePolicy  expirePolicy;
        
        public IUAsset GetUAsset()
        {
            var data = new GameEffectStack();
            data.hashKey = $"{key}{stackPolicy}{maxCount}{durationRefreshPolicy}{periodResetPolicy}{expirePolicy}".GetHashCode();
            data.stackPolicy = stackPolicy;
            data.maxCount = maxCount;
            data.durationRefreshPolicy = durationRefreshPolicy;
            data.periodResetPolicy = periodResetPolicy;
            data.expirePolicy = expirePolicy;
            return data;
        }
    }
}