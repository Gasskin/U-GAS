namespace U_GAS
{
    public class GameAbilityComponent : BaseEntityComponent
    {
        public GameEffectComponent GameEffectComponent { get;private set; }
        public GameAttributeComponent GameAttributeComponent { get;private set; }
        
        protected override void OnStart()
        {
            GameEffectComponent = new();
            GameAttributeComponent = new();
            
            GameAttributeComponent.OnStart(this);
        }

        protected override void OnStop()
        {
        }

        protected override void OnUpdate(float dt)
        {
        }
    }
}