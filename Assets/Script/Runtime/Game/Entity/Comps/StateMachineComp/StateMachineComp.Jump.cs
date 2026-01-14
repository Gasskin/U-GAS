using UnityEngine;

public class StateMachineJump
{
    private bool _isPressed;
    private bool _isReleased;
    private bool _isHolding;

    // 上升中
    private bool _isUp;

    private int _canJump;

    private StateMachineComp _stateMachine;

    public void Initialize(StateMachineComp stateMachine)
    {
        _stateMachine = stateMachine;
        _canJump = _stateMachine.Settings.CanJumpCount;
    }

    public void ReadInput()
    {
        if (_stateMachine.Context.Jump.IsPressedThisFrame)
        {
            _isPressed = true;
        }
        if (_stateMachine.Context.Jump.IsReleasedThisFrame)
        {
            _isReleased = true;
        }
        _isHolding = _stateMachine.Context.Jump.IsHolding;
    }

    public float GetVelocityY()
    {
        var velocity = _stateMachine.Velocity.Velocity;
        _isUp = velocity.y > 0;

        var jump = _isPressed && _canJump > 0;

        if (jump)
        {
            velocity.y = _stateMachine.Settings.MaxJumpVelocity;

            _canJump--;
        }

        if (_isReleased)
        {
            velocity.y = Mathf.Min(velocity.y, _stateMachine.Settings.MinJumpVelocity);
        }
        
        return velocity.y;
    }
}

public partial class StateMachineComp
{
    public StateMachineJump Jump = new();
}