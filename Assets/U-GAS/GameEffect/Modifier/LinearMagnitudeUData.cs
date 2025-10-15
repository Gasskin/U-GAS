using ProtoBuf;

namespace U_GAS
{
    [ProtoContract]
    public class LinearMagnitudeUData : ModifierMagnitudeCalculationUData
    {
        [ProtoMember(1)]
        public float k;
        [ProtoMember(2)]
        public float b;
    }
}