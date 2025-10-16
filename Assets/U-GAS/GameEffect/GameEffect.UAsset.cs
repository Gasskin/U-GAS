using System.Collections.Generic;
using ProtoBuf;
using Sirenix.OdinInspector;

namespace U_GAS
{
    public enum EDurationPolicy
    {
        [LabelText("瞬时")]
        Instant = 0,

        [LabelText("永久")]
        Infinite = 1,

        [LabelText("限时")]
        Duration = 2,
    }

    public enum EModifierOperation
    {
        [LabelText("加")]
        Add = 0,

        [LabelText("减")]
        Minus = 1,

        [LabelText("乘")]
        Multiply = 2,

        [LabelText("除")]
        Divide = 3,

        [LabelText("替")]
        Override = 4,
    }

    public enum EGameEffectStackPolicy
    {
        [LabelText("不堆叠")]
        None = 0,

        [LabelText("来源堆叠")]
        Source = 1,

        [LabelText("目标堆叠")]
        Target = 2,
    }

    public enum EGameEffectStackDurationRefreshPolicy
    {
        None = 0,
        RefreshOnSuccessApply = 1,
    }

    public enum EGameEffectStackPeriodResetPolicy
    {
        None = 0,
        RefreshOnSuccessApply = 1,
    }

    public enum EGameEffectStackExpirePolicy
    {
        RemoveAll = 0,
        RemoveSingleAndRefreshDuration = 1,
        RefreshDuration = 2,
    }

    [ProtoContract]
    public class GameEffectModifier : IUAsset
    {
        [ProtoMember(1)]
        public EGameAttribute attribute;

        [ProtoMember(2)]
        public EModifierOperation operation;

        [ProtoMember(3)]
        public float input;

        [ProtoMember(4)]
        public ModifierMagnitude magnitude;
    }

    [ProtoContract]
    public class GameEffectStack : IUAsset
    {
        [ProtoMember(1)]
        public int hashKey;
        
        [ProtoMember(2)]
        public EGameEffectStackPolicy stackPolicy;

        [ProtoMember(3)]
        public int maxCount;

        [ProtoMember(4)]
        public EGameEffectStackDurationRefreshPolicy durationRefreshPolicy;

        [ProtoMember(5)]
        public EGameEffectStackPeriodResetPolicy periodResetPolicy;
        
        [ProtoMember(6)]
        public EGameEffectStackExpirePolicy  expirePolicy;
    }

    [ProtoContract]
    public partial class GameEffect : IUAsset
    {
        [ProtoMember(1)]
        public EDurationPolicy durationPolicy;

        [ProtoMember(2)]
        public float duration;

        [ProtoMember(3)]
        public float period;

        [ProtoMember(4)]
        public List<string> assetTags;

        [ProtoMember(5)]
        public List<string> grantedTags;

        [ProtoMember(6)]
        public List<string> applyRequiredTags;

        [ProtoMember(7)]
        public List<string> ongoingRequiredTags;

        [ProtoMember(8)]
        public List<string> immuneTags;

        [ProtoMember(9)]
        public List<string> removeGameEffectsWithTags;

        [ProtoMember(10)]
        public List<GameEffectModifier> modifiers;

        [ProtoMember(11)]
        public GameEffect periodGameEffect;

        [ProtoMember(12)]
        public bool needSnapShot;

        [ProtoMember(13)]
        public GameEffectStack stack;
    }
}