using Cysharp.Threading.Tasks;

namespace Script.Runtime.Framework.System
{
    public abstract class EntityComp
    {
    #region Priority
        // >0 是为了保险
        public const int Priority_Gas = 1;
        public const int Priority_View = 2;
        public const int Priority_SkillSpell = 3;
        public const int Priority_BattleInput = 4;
        public const int Priority_StateMachine = 5;
        public const int Priority_Cell = 6;
        public const int Priority_Camp = 7;
    #endregion

        public int Priority { get; private set; }

        public Entity Entity;

        public bool IsValid => Entity.IsValid;

        public virtual bool NeedTick { get; } = false;
        public virtual bool NeedLateTick { get; } = false;
        public virtual bool NeedFixedTick { get; } = false;

        public virtual async UniTask Initialize()
        {
            await UniTask.Yield();
        }

        public virtual void Destroy()
        {
        }

        public virtual void OnTick(float dt)
        {
        }

        public virtual void OnLateTick(float dt)
        {
        }

        public virtual void OnFixedTick(float dt)
        {
        }

        public void SetPriority(int p)
        {
            Priority = p;
        }
    }
}