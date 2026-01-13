using System.Collections.Generic;

public abstract class BaseState
{
    protected StateMachineComp StateMachine { get; private set; }
    
    public void Initialize(StateMachineComp comp)
    {
        StateMachine = comp;
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
    }

    public abstract void OnEnter();
    public abstract void Tick(float dt);
    public abstract void FixedTick(float dt);
    public abstract void OnExit();

    public abstract bool CanEnterTo(BaseState to);
    public abstract bool CanEnter();

    public List<BaseState> ToState = new();
}