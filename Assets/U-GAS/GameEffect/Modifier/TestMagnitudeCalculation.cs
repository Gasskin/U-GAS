using ProtoBuf;

namespace U_GAS
{
    [ProtoContract]
    public class TestMagnitudeCalculation : ModifierMagnitudeCalculation
    {
        [ProtoMember(1)]
        public float k;
        
        public override float CalculateMagnitude(GameEffectSpec spec, float input)
        {
            return k;
        }
    }
}