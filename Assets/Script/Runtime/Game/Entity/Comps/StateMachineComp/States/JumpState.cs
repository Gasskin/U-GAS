using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
        typeof(FallState),
    };

    private Vector2 _velocity = Vector2.zero;

    public override void Initialize(StateMachineComp comp)
    {
        base.Initialize(comp);
        StateMachine.Jump.OnMultiJump += () => { StateMachine.Play(StateMachineComp.StateName_MultiJump); };
    }


    public override void OnEnter()
    {
        StateMachine.Play(StateMachineComp.StateName_Jump);
    }

    public override void Tick(float dt)
    {
        StateMachine.Jump.ReadInput();
        StateMachine.Turn.CheckAndTurn();
    }

    public override void FixedTick(float dt)
    {
        _velocity.y = StateMachine.Jump.GetVelocityY();
        _velocity.y = StateMachine.Fall.GetVelocityY(_velocity);

        StateMachine.Velocity.AddVelocity(_velocity);
    }

    public override void OnExit()
    {
        StateMachine.Jump.Exit();
    }

    protected override bool CanEnterTo(BaseState to)
    {
        switch (to)
        {
            case FallState:
                return StateMachine.Jump.CanFall;
        }
        return false;
    }


    public override bool CanEnter()
    {
        return false;
    }


}