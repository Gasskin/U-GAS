namespace cfg.Gas
{
    public partial class SimpleFloatMagnitude
    {
        public override float Calculate(float inValue)
        {
            return inValue * K + B;
        }
    }
}