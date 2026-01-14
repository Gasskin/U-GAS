using UnityEngine;

public class CheckTurn
{
    public bool IsFacingRight { get; private set; } = true;
    
    private StateMachineComp _stateMachine;
    
    public void Initialize(StateMachineComp machine)
    {
        _stateMachine = machine;
    }
    
    public void CheckAndTurn(Vector2 moveDirection)
    {
        if ((moveDirection.x < 0 && IsFacingRight) || (moveDirection.x > 0 && !IsFacingRight))
        {
            Turn();
        }
    }
    
    private void Turn()
    {
        IsFacingRight = !IsFacingRight;
		
        Vector3 scale = _stateMachine.Settings.Sprite.transform.localScale;
        scale.x = IsFacingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
		
        _stateMachine.Settings.Sprite.transform.localScale = scale;
    }
}

public partial class StateMachineComp
{
    public CheckTurn CheckTurn = new();
}