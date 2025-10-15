using System.Collections.Generic;
using ProtoBuf;
using Sirenix.OdinInspector;

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

    [ProtoContract]
    public class GameEffectModifier : IUAsset
    {
        [ProtoMember(1)]
        public EGameAttribute attribute;

        [ProtoMember(2)]
        public float input;

        [ProtoMember(3)]
        public ModifierMagnitude magnitude;
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
        public List<string> requiredTags;

        [ProtoMember(7)]
        public List<string> conflictTags;

        [ProtoMember(8)]
        public List<string> removeGameEffectsWithTags;

        [ProtoMember(9)]
        public List<GameEffectModifier> modifiers;

        [ProtoMember(10)]
        public GameEffect periodGameEffect;

        [ProtoMember(11)]
        public bool needSnapShot;
    }
}