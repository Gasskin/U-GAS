using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleTimeSystem : BaseSystem , ITickSystem
{
    public float Now { get; private set; }
    
    public override async UniTask Initialize()
    {
        Now = Time.deltaTime;
        await UniTask.Yield();
    }

    public override void Close()
    {
        
    }

    public void Tick(float dt)
    {
        Now += dt;
    }
}