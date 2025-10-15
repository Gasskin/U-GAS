using ProtoBuf;

namespace U_GAS
{
    [ProtoContract]
    public class TestMagnitude : ModifierMagnitude
    {
        [ProtoMember(1)]
        public float k;
        
        public override float CalculateMagnitude(GameEffect.GameEffectSpec spec, float input)
        {
            return k;
        }
    }
}