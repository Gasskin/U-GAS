using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
        typeof(FallState),
    };

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
        StateMachine.Turn.CheckAndTurn();
        StateMachine.Jump.ReadInput();
    }

    public override void FixedTick(float dt)
    {
        var velocity = StateMachine.Jump.CalculateVelocity(StateMachine.Velocity.Velocity);
        
        velocity = StateMachine.Movement.CalculateVelocity(velocity, StateMachine.Context.MoveDir,
            StateMachine.Settings.AirSpeed, StateMachine.Settings.JumpAirAcceleration,
            StateMachine.Settings.JumpAirDeceleration);
        
        velocity = StateMachine.Fall.CalculateVelocity(velocity);

        StateMachine.Velocity.AddVelocity(velocity);
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