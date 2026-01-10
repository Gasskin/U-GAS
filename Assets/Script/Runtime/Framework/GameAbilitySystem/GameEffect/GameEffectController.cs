using cfg.Gas;

public class GameEffectController
{
    public ulong AddGameEffectSpec(GameEffectSpec spec)
    {
        if (!spec.CanApply())
        {
            Pool<GameEffectSpec>.Release(spec);
            return 0;
        }

        if (spec.IsImmune())
        {
            Pool<GameEffectSpec>.Release(spec);
            return 0;
        }
        
        // 瞬时GE
        if (spec.GameEffect.Duration.DurationType == EGameEffectDurationType.Instant)
        {
            spec.OnExecute();
            Pool<GameEffectSpec>.Release(spec);
            return 0;
        }


        return 0;
    }
}