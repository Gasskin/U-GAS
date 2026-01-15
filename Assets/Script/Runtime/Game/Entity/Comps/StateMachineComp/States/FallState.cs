using UnityEngine;

public class FallState : BaseState
{
    private Vector2 _velocity;
    
    public override void OnEnter()
    {
        StateMachine.Animator.Play(StateMachineComp.StateName_Fall);
    }

    public override void Tick(float dt)
    {
        StateMachine.Fall.ReadInput();
    }

    public override void FixedTick(float dt)
    {
        _velocity.y = StateMachine.Fall.GetGravity(StateMachine.Velocity.Velocity);
        
        StateMachine.Velocity.AddVelocity(_velocity);
    }

    public override void OnExit()
    {
    }

    public override bool CanEnterTo(BaseState to)
    {
        switch (to)
        {
            case IdleState:
                return StateMachine.Collision.IsGrounded;
        }
        return false;
    }

    public override bool CanEnter()
    {
        return false;;
    }
}