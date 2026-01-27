using System;
using Cysharp.Threading.Tasks;

namespace Script.Runtime.Framework.System
{
    public class UISystem : BaseSystem, ITickSystem
    {
        public override async UniTask Initialize()
        {
            await UniTask.Yield();
        }

        public override void Destroy()
        {
        }

        public void Tick(float dt)
        {
        }
    }
}