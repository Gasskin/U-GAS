using System;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
        typeof(IdleState),
        typeof(JumpState),
    };

    private Vector2 _velocity;


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
        _velocity.y = StateMachine.Fall.GetVelocityY(StateMachine.Velocity.Velocity);

        StateMachine.Velocity.AddVelocity(_velocity);
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
                       StateMachine.Context.MoveDir == Vector2.zero &&
                       Mathf.Abs(StateMachine.Velocity.Velocity.x) < 0.1f;
            case JumpState:
                if (StateMachine.Context.Jump.IsPressedThisFrame)
                {
                    if (StateMachine.Jump.CanMultiJump)
                    {
                        return true;
                    }
                }
                return false;
        }
        return false;
    }

    public override bool CanEnter()
    {
        return false;
    }
}