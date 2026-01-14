using System;
using UnityEngine;

public class IdleState : BaseState
{
    public override void OnEnter()
    {
        StateMachine.Animator.Play(StateMachineComp.STATE_NAME_IDLE);
    }

    public override void Tick(float dt)
    {
        StateMachine.CheckTurn.CheckAndTurn(StateMachine.Context.MoveDir);
    }

    public override void FixedTick(float dt)
    {
        var velocity = new Vector2(0f, StateMachine.Settings.FallGravity);
        StateMachine.Velocity.AddVelocity(velocity);
    }

    public override void OnExit()
    {
    }

    public override bool CanEnterTo(BaseState to)
    {
        switch (to)
        {
            case FallState fall:
            {
                return !StateMachine.CheckCollision.IsGrounded;
            }
        }
        throw new ArgumentOutOfRangeException($"{GetType().Name} can not change to {to.GetType().Name}");
    }
    
    
    public override bool CanEnter()
    {
        return false;;
    }
}