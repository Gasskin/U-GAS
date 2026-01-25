using System;
using System.Collections.Generic;
using Animancer;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// State.FixedTick -> AddVelocity
/// Velocity.FixedTick -> ApplyAndClear
/// 
/// BattleInput.CallBack (BeforeUpdated) -> SaveInput
/// State.Tick -> Check Enter
/// State.Tick -> ReadInput
/// 
/// Context.LateTick -> ClearInput
/// </summary>
public partial class StateMachineComp : EntityComp
{
    public override int Priority => Priority_StateMachine;
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
        Entity.HasComp(Priority_View,out GameObjectComp view);

        // Animator  = view.View.GetComponentInChildren<Animator>();
        Animancer  = view.View.GetComponentInChildren<AnimancerComponent>();
        Settings = view.View.GetComponent<StateMachineSetting>();
        
        // Collision.Initialize(this);
        Turn.Initialize(this);
        // Dash.Initialize(this);
        Velocity.Initialize(this);
        // Jump.Initialize(this);
        // Fall.Initialize(this);
        Movement.Initialize(this);

        for (int i = 0; i < _states.Count; i++)
        {
            _states[i].Initialize(this);
        }
        ChangeState(_stateDic[typeof(IdleState)]);
        await UniTask.Yield();
    }

    public override void Tick(float dt)
    {
        // Collision.Tick(dt);
        // Dash.Tick(dt);

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
            var state  = _curState.GetCanEnterTo();
            if (state != null) 
            {
                ChangeState(state);
            }
        }
        _curState?.Tick(dt);
    }

    public override void FixedTick(float dt)
    {
        _curState?.FixedTick(dt);
        Velocity.FixedTick();
    }

    public override void LateTick(float dt)
    {
        Context.LateTick(dt);
    }

    public BaseState GetState(Type type)
    {
        return _stateDic.GetValueOrDefault(type, null);
    }

    public bool IsState<T>(out T state) where T : BaseState
    {
        state = null;
        if (_curState is T t)
        {
            state = t;
            return true;
        }
        return false;
    }

    public void ChangeState<T>() where T : BaseState
    {
        if (_stateDic.TryGetValue(typeof(T), out var state))
        {
            ChangeState(state);
        }
    }

    private void ChangeState(BaseState state)
    {
        if (state == _curState)
        {
            return;
        }
// #if UNITY_EDITOR
//         if (_curState != null)
//         {
//             Debug.Log($"{_curState.GetType().Name} Exit");
//         }
// #endif
        _curState?.OnExit();
        _curState = state;
// #if UNITY_EDITOR
//         if (_curState != null)
//         {
//             Debug.Log($"{_curState.GetType().Name} Enter");
//         }
// #endif
        _curState?.OnEnter();
    }
}