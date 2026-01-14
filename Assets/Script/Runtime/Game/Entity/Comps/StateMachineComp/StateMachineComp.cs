using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public partial class StateMachineComp : EntityComp
{
    public override int Priority => STATE_MACHINE;
    public override bool NeedTick => true;

    public override bool NeedFixedTick => true;

    public override bool NeedLateTick => true;

    public StateMachineSetting Settings { get; private set; }
    
    // state machine
    private BaseState _curState;

    private readonly Dictionary<Type, BaseState> _stateDic = new();

    private readonly List<BaseState> _states;

    public StateMachineComp(params BaseState[] inStates)
    {
        _states = new List<BaseState>();
        _states.AddRange(inStates);
        foreach (var state in _states)
        {
            _stateDic.Add(state.GetType(), state);
        }
    }

    public override async UniTask Initialize()
    {
        Entity.HasComp(VIEW,out ViewComp view);

        Animator  = view.View.GetComponentInChildren<Animator>();
        Settings = view.View.GetComponent<StateMachineSetting>();
        
        Collision.Initialize(this);
        Turn.Initialize(this);
        Velocity.Initialize(this);
        Jump.Initialize(this);
        Fall.Initialize(this);

        for (int i = 0; i < _states.Count; i++)
        {
            _states[i].Initialize(this);
        }
        ChangeState(_stateDic[typeof(IdleState)]);
        await UniTask.Yield();
    }

    public override void Tick(float dt)
    {
        Collision.TickCheck(dt);

        var changeAny = false;
        for (int i = 0; i < _states.Count; i++)
        {
            if (_curState != _states[i] && _states[i].CanEnter())
            {
                ChangeState(_states[i]);
                changeAny = true;
            }
        }

        if (!changeAny && _curState != null)
        {
            for (int i = 0; i < _curState.ToState.Count; i++)
            {
                if (_curState.CanEnterTo(_curState.ToState[i]))
                {
                    ChangeState(_curState.ToState[i]);
                }
            }
        }
        _curState?.Tick(dt);
    }

    public override void FixedTick(float dt)
    {
        Velocity.FixedTick();
        
        _curState?.FixedTick(dt);
    }

    public override void LateTick(float dt)
    {
        Context.LateTick(dt);
    }


    private void ChangeState(BaseState state)
    {
        if (state == _curState)
        {
            return;
        }
        _curState?.OnExit();
        _curState = state;
        _curState?.OnEnter();
    }
}