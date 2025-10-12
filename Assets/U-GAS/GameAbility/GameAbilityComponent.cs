namespace U_GAS
{
    public class GameAbilityComponent : BaseEntityComponent
    {
        public GameEffectContainer GameEffectContainer { get;private set; }
        public GameAttributeContainer GameAttributeContainer { get;private set; }
        
        protected override void OnStart()
        {
            GameEffectContainer = new();
            GameAttributeContainer = new();
            
            GameAttributeContainer.OnStart(this);
        }

        protected override void OnStop()
        {
        }

        protected override void OnUpdate(float dt)
        {
        }
    }
}