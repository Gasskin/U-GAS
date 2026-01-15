using UnityEngine;

public class StateMachineFall
{
    private StateMachineComp _stateMachine;

    private bool _isJumpHolding;

    public void Initialize(StateMachineComp stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void ReadInput()
    {
        _isJumpHolding = _stateMachine.Context.Jump.IsHolding;
    }

    public float GetGravity(Vector2 currentVelocity)
    {
        var gravityMultiplier = _stateMachine.Settings.FallGravityMultiplayer;
        if (Mathf.Abs(currentVelocity.y) <= _stateMachine.Settings.HangJumpVelocity)
        {
            gravityMultiplier = _stateMachine.Settings.HangJumpGravityMultiplayer;
        }
        else if (_isJumpHolding)
        {
            gravityMultiplier = _stateMachine.Settings.HoldJumpGravityMultiplayer;
        }
        currentVelocity.y -= _stateMachine.Settings.Gravity * gravityMultiplier * Time.fixedDeltaTime;

        currentVelocity.y = Mathf.Clamp(currentVelocity.y, _stateMachine.Settings.MaxFallSpeed, _stateMachine.Settings.MaxUpwardSpeed);

        return currentVelocity.y;
    }
}

public partial class StateMachineComp
{
    public StateMachineFall Fall = new();
}