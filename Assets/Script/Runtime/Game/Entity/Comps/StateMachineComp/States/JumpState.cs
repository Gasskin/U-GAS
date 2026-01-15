using cfg;
using UnityEngine;

public class JumpState : BaseState
{
    private Vector2 _velocity = Vector2.zero;
    
    public override void OnEnter()
    {
        StateMachine.Animator.Play(StateMachineComp.StateName_Jump);
    }

    public override void Tick(float dt)
    {
        StateMachine.Jump.ReadInput();
        StateMachine.Turn.CheckAndTurn();
    }

    public override void FixedTick(float dt)
    {
        _velocity.y = StateMachine.Jump.GetVelocityY();
        _velocity.y = StateMachine.Fall.GetGravity(_velocity);
        
        StateMachine.Velocity.AddVelocity(_velocity);
    }

    public override void OnExit()
    {
    }

    public override bool CanEnterTo(BaseState to)
    {
        switch (to)
        {
            case FallState:
                return StateMachine.Velocity.Velocity.y < 0;
        }
        return false;
    }
    
    
    public override bool CanEnter()
    {
        return false;;
    }
}