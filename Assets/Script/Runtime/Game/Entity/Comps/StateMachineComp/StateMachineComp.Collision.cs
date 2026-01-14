using UnityEngine;

public class StateMachineCollision
{
    public bool IsGrounded { get; private set; }

    private StateMachineComp _stateMachine;
    
    public void Initialize(StateMachineComp machine)
    {
        _stateMachine = machine;
    }

    public void TickCheck(float dt)
    {
        CheckIsGrounded();
    }

    private void CheckIsGrounded()
    {
        IsGrounded = false;
        var bounds = _stateMachine.Settings.Foot.bounds;
        var boxCastOrigin = new Vector2(bounds.center.x, bounds.min.y);
        var boxCastSize = new Vector2(bounds.size.x, _stateMachine.Settings.GroundDetectionRayLength);

        IsGrounded = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, _stateMachine.Settings.GroundDetectionRayLength, 1 << LayerMask.NameToLayer("Ground")).collider != null;
    }
}

public partial class StateMachineComp
{
    public StateMachineCollision Collision = new();
}