using System;
using System.Collections.Generic;
public class RunState: BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
    };
    
    public override void OnEnter()
    {
    }

    public override void Tick(float dt)
    {
    }

    public override void FixedTick(float dt)
    {
    }

    public override void OnExit()
    {
    }

    protected override bool CanEnterTo(BaseState to)
    {
        return false;
    }
    
    
    public override bool CanEnter()
    {
        return false;;
    }
}