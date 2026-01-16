using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
        typeof(JumpState),
        typeof(RunState),
    };

    public override void OnEnter()
    {
        StateMachine.Play(StateMachineComp.StateName_Idle);
    }

    public override void Tick(float dt)
    {
        StateMachine.Turn.CheckAndTurn();
    }

    public override void FixedTick(float dt)
    {
        var velocity = new Vector2(0f, StateMachine.Settings.GroundGravity);
        StateMachine.Velocity.AddVelocity(velocity);
    }

    public override void OnExit()
    {
    }

    protected override bool CanEnterTo(BaseState to)
    {
        switch (to)
        {
            case JumpState:
                return StateMachine.Context.Jump.IsPressedThisFrame;
            case RunState:
                return StateMachine.Context.MoveDir != 0 && !StateMachine.Collision.IsTouchWall;
        }
        return false;
    }
    
    
    public override bool CanEnter()
    {
        return false;;
    }
}