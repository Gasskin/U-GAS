using UnityEngine;

public class StateMachineCollision
{
    public event System.Action OnGroundTouched; 
    public event System.Action OnGroundLeft; 
    
    public bool IsGrounded { get; private set; }
    private bool _isGrounded;
    
    
    private StateMachineComp _stateMachine;
    
    public void Initialize(StateMachineComp machine)
    {
        _stateMachine = machine;
    }

    public void TickCheck(float dt)
    {
        CheckIsGrounded();
        
        
        CheckStateChange();
    }

    private void CheckIsGrounded()
    {
        IsGrounded = false;
        var bounds = _stateMachine.Settings.Foot.bounds;
        var boxCastOrigin = new Vector2(bounds.center.x, bounds.min.y);
        var boxCastSize = new Vector2(bounds.size.x, _stateMachine.Settings.GroundDetectionRayLength);

        IsGrounded = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, _stateMachine.Settings.GroundDetectionRayLength, 1 << LayerMask.NameToLayer("Ground")).collider != null;
    }

    private void CheckStateChange()
    {
        // 回到地面
        if (IsGrounded && !_isGrounded)
        {
            OnGroundTouched?.Invoke();
        }
        else if (!IsGrounded && _isGrounded)
        {
            OnGroundLeft?.Invoke();
        }
        
        _isGrounded = IsGrounded;
    }
}

public partial class StateMachineComp
{
    public StateMachineCollision Collision = new();
}