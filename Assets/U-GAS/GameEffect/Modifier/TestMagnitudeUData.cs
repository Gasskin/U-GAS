using ProtoBuf;

namespace U_GAS
{
    [ProtoContract]
    public class TestMagnitudeUData: ModifierMagnitudeCalculationUData
    {
        [ProtoMember(1)]
        public float k;
    }
}