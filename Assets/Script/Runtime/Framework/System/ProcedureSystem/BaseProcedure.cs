namespace Script.Runtime.Framework.System
{
    public abstract class BaseProcedure
    {
        public abstract void Enter();
        public abstract void Tick(float dt);
        public abstract void Exit();
    }
}