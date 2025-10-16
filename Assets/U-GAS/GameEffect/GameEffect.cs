namespace U_GAS
{
    public partial class GameEffect
    {
        public GameEffectSpec CreateSpec(GameAbilityComponent source, GameAbilityComponent target)
        {
            var spec = UPool<GameEffectSpec>.Get();
            spec.Init(this, source, target);
            return spec;
        }

        public bool StackEqualTo(GameEffect effect)
        {
            if (stack.stackPolicy == EGameEffectStackPolicy.None || effect.stack.stackPolicy == EGameEffectStackPolicy.None)
            {
                return false;
            }
            if (stack.hashKey == 0 || effect.stack.hashKey == 0)
            {
                return false;
            }

            return stack.hashKey == effect.stack.hashKey;
        }
    }
}