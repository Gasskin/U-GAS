using System;
using System.Collections.Generic;

public class RunState : BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
        typeof(RunJumpState),
        typeof(IdleState),
    };

    public override void OnEnter()
    {
        StateMachine.Play(StateMachineComp.StateName_Run);
    }

    public override void Tick(float dt)
    {
        StateMachine.Turn.CheckAndTurn();
    }

    public override void FixedTick(float dt)
    {
        var velocity = StateMachine.Movement.CalculateVelocity(StateMachine.Velocity.Velocity,
            StateMachine.Context.MoveDir,
            StateMachine.Settings.RunSpeed, StateMachine.Settings.RunAcceleration,
            StateMachine.Settings.RunAcceleration);
        
        velocity.y = StateMachine.Settings.GroundGravity;

        StateMachine.Velocity.AddVelocity(velocity);
    }


    public override void OnExit()
    {
    }

    protected override bool CanEnterTo(BaseState to)
    {
        switch (to)
        {
            case RunJumpState:
                if (StateMachine.Context.Jump.IsPressedThisFrame)
                {
                    return true;
                }
                break;
            case IdleState:
                if (StateMachine.Collision.IsGrounded && StateMachine.Context.MoveDir == 0 &&
                    StateMachine.Velocity.NoHorizontalVelocity)
                {
                    return true;
                }
                if (StateMachine.Collision.IsTouchWall)
                {
                    return true;
                }
                break;
        }
        return false;
    }


    public override bool CanEnter()
    {
        return false;
        ;
    }
}