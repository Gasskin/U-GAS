using System;
using System.Collections.Generic;

public abstract class BaseState
{
    protected StateMachineComp StateMachine { get; private set; }

    protected StateMachineSetting Settings => StateMachine.Settings;

    protected abstract List<Type> CheckToStates { get; }
    
    public virtual void Initialize(StateMachineComp comp)
    {
        StateMachine = comp;
        foreach (var s in CheckToStates)
        {
            var get = StateMachine.GetState(s);
            if (get != null) 
            {
                _toState.Add(get);
            }
        }
    }

    public abstract void OnEnter();
    public abstract void Tick(float dt);
    public abstract void FixedTick(float dt);
    public abstract void OnExit();

    protected abstract bool CanEnterTo(BaseState to);
    public abstract bool CanEnter();

    private List<BaseState> _toState = new();

    public BaseState GetCanEnterTo()
    {
        for (int i = 0; i < _toState.Count; i++)
        {
            var to = _toState[i];
            if (CanEnterTo(to))
            {
                return to;
            }
        }
        return null;
    }
}