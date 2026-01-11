using UnityEngine;

public class BattleTimeSystem : BaseSystem , ITickSystem
{
    public float Now { get; private set; }
    
    public override void Initialize()
    {
        Now = Time.deltaTime;
    }

    public override void Close()
    {
        
    }

    public void Tick(float dt)
    {
        Now += dt;
    }
}