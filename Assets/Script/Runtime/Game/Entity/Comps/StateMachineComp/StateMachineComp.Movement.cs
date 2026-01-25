using System;
using UnityEngine;

public class StateMachineMovement
{
    private StateMachineComp _stateMachine;


    public void Initialize(StateMachineComp stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public Vector2 CalculateVelocity(Vector2 inVelocity, float speed, float acceleration, float deceleration)
    {
        var input = _stateMachine.Context.MoveDir;

        var velocity = new Vector2(input.x * speed, input.y * speed);
        var ax = input.x != 0 ? acceleration : deceleration;
        // var ay = input.y != 0 ? acceleration : deceleration;

        inVelocity.x = Mathf.MoveTowards(inVelocity.x, velocity.x, ax * Time.fixedDeltaTime);
        // inVelocity.y = Mathf.MoveTowards(inVelocity.y, velocity.y, ay * Time.fixedDeltaTime);

        return inVelocity;
    }


    private void CalculateVelocity(int dir, float speed, float acceleration, float deceleration)
    {
    }
}

public partial class StateMachineComp
{
    public StateMachineMovement Movement = new();
}