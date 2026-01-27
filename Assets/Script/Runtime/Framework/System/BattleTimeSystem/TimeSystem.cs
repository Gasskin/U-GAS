using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    public class TimeSystem : BaseSystem , ITickSystem
    {
        public float Now { get; private set; }
    
        public override async UniTask Initialize()
        {
            Now = Time.realtimeSinceStartup;
            await UniTask.Yield();
        }

        public override void Destroy()
        {
        
        }

        public void Tick(float dt)
        {
            Now += dt;
        }
    }
}