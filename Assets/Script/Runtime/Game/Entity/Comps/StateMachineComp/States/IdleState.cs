using System;

public class IdleState : BaseState
{
    public override void OnEnter()
    {
        StateMachine.Animator.Play(StateMachineComp.STATE_NAME_IDLE);
    }

    public override void Tick(float dt)
    {
    }

    public override void FixedTick(float dt)
    {
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
                return !StateMachine.IsGrounded;
            }
        }
        throw new ArgumentOutOfRangeException($"{GetType().Name} can not change to {to.GetType().Name}");
    }
    
    
    public override bool CanEnter()
    {
        return false;;
    }
}