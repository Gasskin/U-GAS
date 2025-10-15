using ProtoBuf;

namespace U_GAS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(LinearMagnitude))] 
    [ProtoInclude(101, typeof(TestMagnitude))] 
    public class ModifierMagnitude : IUAsset
    {
        public virtual float CalculateMagnitude(GameEffect.GameEffectSpec spec, float input)
        {
            return 0;
        }
    }
}