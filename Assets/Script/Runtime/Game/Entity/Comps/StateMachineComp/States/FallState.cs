using System;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
        typeof(IdleState),
        typeof(JumpState),
        typeof(RunState),
    };


    public override void OnEnter()
    {
        StateMachine.Play(StateMachineComp.StateName_Fall);
    }

    public override void Tick(float dt)
    {
        StateMachine.Fall.ReadInput();
        StateMachine.Turn.CheckAndTurn();
    }

    public override void FixedTick(float dt)
    {
        var velocity = StateMachine.Fall.CalculateVelocity(StateMachine.Velocity.Velocity);
        velocity = StateMachine.Movement.CalculateVelocity(velocity, StateMachine.Context.MoveDir,
            StateMachine.Settings.AirSpeed, StateMachine.Settings.JumpAirAcceleration,
            StateMachine.Settings.JumpAirDeceleration);
        StateMachine.Velocity.AddVelocity(velocity);
    }

    public override void OnExit()
    {
    }

    protected override bool CanEnterTo(BaseState to)
    {
        switch (to)
        {
            case IdleState:
                return StateMachine.Collision.IsGrounded &&
                       StateMachine.Context.MoveDir == 0 &&
                       StateMachine.Velocity.NoHorizontalVelocity;
            case JumpState:
                return StateMachine.Context.Jump.IsPressedThisFrame &&
                       StateMachine.Jump.CanMultiJump;
            case RunState:
                return StateMachine.Collision.IsGrounded &&
                       StateMachine.Context.MoveDir != 0;
        }
        return false;
    }

    public override bool CanEnter()
    {
        return false;
    }
}