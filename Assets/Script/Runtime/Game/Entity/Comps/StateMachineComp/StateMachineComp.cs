using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public struct StateMachineContext
{
}

public partial class StateMachineComp : EntityComp
{
    public override int Priority => STATE_MACHINE;
    public override bool NeedTick => true;

    public override bool NeedFixedTick => true;
    
    // state machine
    private BaseState curState;

    private readonly Dictionary<Type, BaseState> stateDic = new();

    private readonly List<BaseState> states;

    public StateMachineComp(params BaseState[] inStates)
    {
        states = new List<BaseState>();
        states.AddRange(inStates);
        foreach (var state in states)
        {
            stateDic.Add(state.GetType(), state);
        }
    }

    public override async UniTask Initialize()
    {
        Entity.HasComp(VIEW,out ViewComp view);

        Animator  = view.View.GetComponentInChildren<Animator>();
        setting = view.View.GetComponent<StateMachineSetting>();

        for (int i = 0; i < states.Count; i++)
        {
            states[i].Initialize(this);
        }
        ChangeState(stateDic[typeof(IdleState)]);
        await UniTask.Yield();
    }

    public override void Tick(float dt)
    {
        TickCheck(dt);
        
        for (int i = 0; i < states.Count; i++)
        {
            if (curState != states[i] && states[i].CanEnter())
            {
                ChangeState(states[i]);
                return;
            }
        }

        if (curState != null)
        {
            for (int i = 0; i < curState.ToState.Count; i++)
            {
                if (curState.CanEnterTo(curState.ToState[i]))
                {
                    ChangeState(curState.ToState[i]);
                    return;
                }
            }
        }
        curState?.Tick(dt);
    }

    public override void FixedTick(float dt)
    {
        curState?.FixedTick(dt);
    }


    private void ChangeState(BaseState state)
    {
        if (state == curState)
        {
            return;
        }
        curState?.OnExit();
        curState = state;
        curState?.OnEnter();
    }
}