using System;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;

namespace Script.Runtime.Framework.System
{
    public abstract class EntityComp : IPoolObject
    {
    #region Priority
        // >0 是为了保险
        protected const int Priority_Gas = 1;
        protected const int Priority_GameObject = 2;
        protected const int Priority_SkillSpell = 3;
        protected const int Priority_BattleInput = 4;
        protected const int Priority_StateMachine = 5;
        protected const int Priority_Camp = 6;
        protected const int Priority_HurtBody = 7;
    #endregion

        public int Priority { get; private set; }

        public Entity Entity;

        public virtual bool NeedTick { get; } = false;
        public virtual bool NeedLateTick { get; } = false;
        public virtual bool NeedFixedTick { get; } = false;

        protected bool IsValid => Entity.IsValid;
        
        public virtual async UniTask Initialize()
        {
            await UniTask.Yield();
        }

        public virtual void Destroy()
        {
        }
        
        public void OnRelease()
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