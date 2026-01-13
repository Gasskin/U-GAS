public class FallState : BaseState
{
    public override void OnEnter()
    {
        StateMachine.Animator.Play(StateMachineComp.STATE_NAME_FALL);
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
            case IdleState idle:
                return StateMachine.IsGrounded;
        }
        return false;
    }

    public override bool CanEnter()
    {
        return false;;
    }
}