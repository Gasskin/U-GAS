using System;
using UnityEngine;

public class StateMachineMovement
{
    private StateMachineComp _stateMachine;


    public void Initialize(StateMachineComp stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public Vector2 CalculateVelocity(Vector2 inVelocity, int moveDir, float speed, float acceleration, float deceleration)
    {
        var velocity = moveDir * _stateMachine.Settings.RunSpeed;
        var smooth = moveDir != 0 ? _stateMachine.Settings.RunAcceleration : _stateMachine.Settings.RunDeceleration;

        inVelocity.x = Mathf.MoveTowards(inVelocity.x, velocity, smooth * Time.fixedDeltaTime);

        return inVelocity;
    }
}

public partial class StateMachineComp
{
    public StateMachineMovement Movement = new();
}