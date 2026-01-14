using UnityEngine;

public class StateMachineVelocity
{
    public Vector2 Velocity => _rg.linearVelocity;

    private Vector2 _velocity;

    private StateMachineComp _stateMachine;
    private Rigidbody2D _rg;

    public void Initialize(StateMachineComp machine)
    {
        _stateMachine = machine;
        _rg = _stateMachine.Settings.Rg;
    }

    public void AddVelocity(Vector2 velocity)
    {
        _velocity += velocity;
    }

    public void FixedTick()
    {
        _rg.linearVelocity = _velocity;
        _velocity = Vector2.zero;
    }
}

public partial class StateMachineComp
{
    public StateMachineVelocity Velocity = new();
}