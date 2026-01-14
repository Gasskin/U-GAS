using UnityEngine;

public class StateMachineVelocity
{
    public Vector2 Velocity;

    public float VelocityX => Velocity.x;
    public float VelocityY => Velocity.y;

    public float VelocityAbsX => Mathf.Abs(VelocityX);
    public float VelocityAbsY => Mathf.Abs(VelocityY);
    
    private StateMachineComp _stateMachine;
    
    public void Initialize(StateMachineComp machine)
    {
        _stateMachine = machine;
    }

    public void AddVelocity(Vector2 velocity)
    {
        Velocity += velocity;
    }

    public void FixedTick(float dt)
    {
        _stateMachine.Settings.Rg.linearVelocity = Velocity;
        Velocity= Vector2.zero;
    }
}

public partial class StateMachineComp
{
    public StateMachineVelocity Velocity = new();
}