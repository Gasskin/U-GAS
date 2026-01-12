namespace cfg.Gas
{
    public abstract partial class BaseMagnitude
    {
        public virtual bool IsMetaMagnitude { get; } = false;
        public abstract float Calculate(GameEffectSpec spec);
    }
}