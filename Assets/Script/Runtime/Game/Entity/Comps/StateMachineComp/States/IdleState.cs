using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
        typeof(JumpState),
    };

    public override void OnEnter()
    {
        StateMachine.Animator.Play(StateMachineComp.StateName_Idle);
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
            // case FallState fall:
            // {
            //     return !StateMachine.Collision.IsGrounded;
            // }
            case JumpState:
            {
                return StateMachine.Context.Jump.IsPressedThisFrame;
            }
        }
        return false;
    }
    
    
    public override bool CanEnter()
    {
        return false;;
    }
}