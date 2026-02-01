using System;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.System;

namespace Script.Runtime.Game.Entity
{
    [EntityCompPriority(Priority_HurtBody)]
    public class HurtBodyComp : EntityComp
    {
        private HurtBodyMono _hurtBody;

        public override async UniTask Initialize()
        {
            if (Entity.HasComp(out GameObjectComp gameObjectComp) &&
                gameObjectComp.View.TryGetComponent(out _hurtBody))
            {
                foreach (var body in _hurtBody.HurtBodies)
                {
                    SystemDriver.ColliderSystem.RegisterHurt(body, Entity.Id);
                }
            }
            await UniTask.Yield();
        }

        public override void Destroy()
        {
            if (_hurtBody != null)
            {
                foreach (var body in _hurtBody.HurtBodies)
                {
                    SystemDriver.ColliderSystem.UnRegisterHurt(body, Entity.Id);
                }
            }
        }

    }
}