namespace U_GAS
{
    public abstract class BaseEntityComponent
    {
        public void Start()
        {
            OnStart();
        }

        public void Stop()
        {
            OnStop();
        }
        
        public void DoUpdate(float dt)
        {
            OnUpdate(dt);
        }
        
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract void OnUpdate(float dt);
    }
}