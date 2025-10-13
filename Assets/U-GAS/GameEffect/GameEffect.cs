using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace U_GAS
{
    public enum EDurationPolicy
    {
        [LabelText("瞬时")]
        Instant,

        [LabelText("永久")]
        Infinite,

        [LabelText("限时")]
        Duration
    }


    [Serializable]
    public class GameEffect
    {
        [Title("备注")][HideLabel]
        [TextArea]
        [SerializeField]
        public string backUp;

        [Title("类型")]
        [InfoBox(@"1.瞬时：释放GE时立刻生效1次，随后立刻删除，此时持续时间与生效周期没有意义
2.永久：释放GE时立刻生效1次，随后每个生效周期生效1次，生效周期为0时仅生效1次
3.限时：释放GE时立刻生效1次，随后每个生效周期生效1次，到达持续时间后删除")]
        [LabelText("持续类型")]
        [LabelWidth(80)]
        [SerializeField]
        protected EDurationPolicy durationPolicy = EDurationPolicy.Instant;

        public EDurationPolicy DurationPolicy => durationPolicy;

        [LabelText("持续时间")]
        [LabelWidth(80)]
        [SerializeField]
        protected float duration;

        public float Duration => duration;

        [LabelText("生效周期")]
        [LabelWidth(80)]
        [SerializeField]
        protected float period;

        public float Period => period;
        
        [Title("标签")]
        [LabelText("该GE的标签 - 判断移除GE时，会采用这个标签作为依据")]
        [LabelWidth(80)]
        [SerializeField]
        public List<EGameTag> assetTags;
        [LabelText("该GE会附加的标签 - 生效时添加，失效时移除")]
        [LabelWidth(80)]
        [SerializeField]
        public List<EGameTag> grantedTags;
        [LabelText("应用该GE所必须的标签 - 必须拥有全部标签才可以应用该GE")]
        [LabelWidth(80)]
        [SerializeField]
        public List<EGameTag> requiredTags;
        [LabelText("应用该GE所冲突的标签 - 只要存在任一标签则不可应用该GE")]
        [LabelWidth(80)]
        [SerializeField]
        public List<EGameTag> conflictTags;
        [LabelText("GE生效时移除含有以下任一标签的其他GE - 周期性GE每次生效都会尝试删除其他")]
        [LabelWidth(80)]
        [SerializeField]
        public List<EGameTag> removeGameEffectsWithTags;
    }
}