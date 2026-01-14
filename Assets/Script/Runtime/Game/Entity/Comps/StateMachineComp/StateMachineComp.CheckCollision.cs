using UnityEngine;

public class CheckCollision
{
    public bool IsGrounded { get; private set; }

    private StateMachineSetting setting;

    public void Initialize(StateMachineSetting pSetting)
    {
        setting = pSetting;
    }

    public void TickCheck(float dt)
    {
        CheckIsGrounded();
    }

    private void CheckIsGrounded()
    {
        IsGrounded = false;
        var bounds = setting.Foot.bounds;
        var boxCastOrigin = new Vector2(bounds.center.x, bounds.min.y);
        var boxCastSize = new Vector2(bounds.size.x, setting.GroundDetectionRayLength);

        IsGrounded = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, setting.GroundDetectionRayLength, 1 << LayerMask.NameToLayer("Ground")).collider != null;
    }
}

public partial class StateMachineComp
{
    public CheckCollision CheckCollision = new();
}