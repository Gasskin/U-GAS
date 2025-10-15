using ProtoBuf;

namespace U_GAS
{
    [ProtoContract]
    public class LinearMagnitude : ModifierMagnitude
    {
        [ProtoMember(1)]
        public float k;
        [ProtoMember(2)]
        public float b;

        public override float CalculateMagnitude(GameEffect.GameEffectSpec spec, float input)
        {
            return input * k + b;
        }
    }
}