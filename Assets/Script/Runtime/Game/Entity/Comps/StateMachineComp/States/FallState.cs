using UnityEngine;

public class FallState : BaseState
{
    public override void OnEnter()
    {
        StateMachine.Animator.Play(StateMachineComp.StateName_Fall);
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
        return false;
    }

    public override bool CanEnter()
    {
        return false;;
    }
}