namespace cfg.Gas
{
    public partial class RelativeAttrMagnitude
    {
        public override float Calculate(GameEffectSpec spec)
        {
            var value = spec.Target.GameAttributeController.GetCurrentValue(RelativeAttrBase);
            var k = spec.Target.GameAttributeController.GetCurrentValue(RelativeAttrK);
            var b = spec.Target.GameAttributeController.GetCurrentValue(RelativeAttrB);
            return value * (1 + k) + b;
        }
    }
}