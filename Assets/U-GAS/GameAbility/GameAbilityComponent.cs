using System;

namespace U_GAS
{
    public class GameAbilityComponent : BaseEntityComponent
    {
        public GameEffectComponent GameEffectComponent { get; private set; }
        public GameAttributeComponent GameAttributeComponent { get; private set; }

        public GameTagComponent GameTagComponent { get; private set; }

        protected override void OnStart()
        {
            GameEffectComponent = new();
            GameAttributeComponent = new();
            GameTagComponent = new();

            GameEffectComponent.OnStart(this);
            GameAttributeComponent.OnStart(this);
            GameTagComponent.OnStart();
        }

        protected override void OnStop()
        {
        }

        protected override void OnUpdate(float dt)
        {
        }

        public void ApplyGameEffectTo(GameEffect effect, GameAbilityComponent target)
        {
            var spec = effect.CreateSpec(this, target);
            target.GameEffectComponent.AddGameEffectSpec(spec);
        }

        public void ApplyModFromInstantGameEffect(GameEffect.GameEffectSpec spec)
        {
            foreach (var modifier in spec.GameEffect.modifiers)
            {
                var attr = GameAttributeComponent.GetAttribute(modifier.attribute);
                if (attr.CalculateMode != ECalculateMode.Stacking)
                {
                    throw new InvalidOperationException("Instant GameEffect Can Only Modify Stacking Mode Attribute! ");
                }
                var magnitude = modifier.magnitude.CalculateMagnitude(spec, modifier.input);
                var newValue = attr.BaseValue;
                switch (modifier.operation)
                {
                    case EModifierOperation.Add:
                        newValue += magnitude;
                        break;
                    case EModifierOperation.Minus:
                        newValue -= magnitude;
                        break;
                    case EModifierOperation.Multiply:
                        newValue *= magnitude;
                        break;
                    case EModifierOperation.Divide:
                        newValue /= magnitude;
                        break;
                    case EModifierOperation.Override:
                        newValue = magnitude;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                attr.SetBaseValue(newValue);
            }
        }
    }
}