using ProtoBuf;

namespace U_GAS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(LinearMagnitudeCalculation))] 
    [ProtoInclude(101, typeof(TestMagnitudeCalculation))] 
    public class ModifierMagnitudeCalculation : IUAsset
    {
        public virtual float CalculateMagnitude(GameEffectSpec spec, float input)
        {
            return 0;
        }
    }
}