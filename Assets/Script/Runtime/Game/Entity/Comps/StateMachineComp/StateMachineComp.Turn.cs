using UnityEngine;

public class StateMachineTurn
{
    public bool IsFacingRight { get; private set; } = true;
    
    private StateMachineComp _stateMachine;
    
    public void Initialize(StateMachineComp machine)
    {
        _stateMachine = machine;
    }
    
    public void CheckAndTurn()
    {
        var moveDirection = _stateMachine.Context.MoveDir;
        if ((moveDirection < 0 && IsFacingRight) || (moveDirection > 0 && !IsFacingRight))
        {
            Turn();
        }
    }
    
    private void Turn()
    {
        IsFacingRight = !IsFacingRight;
		
        Vector3 scale = _stateMachine.Settings.transform.localScale;
        scale.x = IsFacingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
		
        _stateMachine.Settings.transform.localScale = scale;
    }
}

public partial class StateMachineComp
{
    public StateMachineTurn Turn = new();
}