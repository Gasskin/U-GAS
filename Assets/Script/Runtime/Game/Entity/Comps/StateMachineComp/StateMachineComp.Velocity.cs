using UnityEngine;

public partial class StateMachineComp
{
    public class StateMachineVelocity
    {
        public Vector2 Velocity => _rg.linearVelocity;
    
        public bool NoVelocity => Mathf.Abs(_rg.linearVelocity.x) <= 0.01f && Mathf.Abs(_rg.linearVelocity.y) <= 0.01f;

        private Vector2 _velocity;

        private StateMachineComp _stateMachine;
        private Rigidbody2D _rg;

        public StateMachineVelocity(StateMachineComp machine)
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
    
    public StateMachineVelocity Velocity;
}