using ProtoBuf;

namespace U_GAS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(LinearMagnitudeUData))] 
    [ProtoInclude(101, typeof(TestMagnitudeUData))] 
    public class ModifierMagnitudeCalculationUData : IUData
    {
        
    }
}