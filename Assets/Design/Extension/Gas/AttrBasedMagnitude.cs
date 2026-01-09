namespace cfg.Gas
{
    public partial class AttrBasedMagnitude
    {
        public override float Calculate(float inValue)
        {
            return Attr * K + B;
        }
    }
}