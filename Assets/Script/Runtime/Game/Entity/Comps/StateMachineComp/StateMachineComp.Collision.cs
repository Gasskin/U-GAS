using System;
using UnityEngine;

public class StateMachineCollision
{
    public event System.Action OnGroundTouched;
    public event System.Action OnGroundLeft;

    public bool IsGrounded { get; private set; }
    private bool _isGrounded;

    public bool IsTouchWall { get; private set; }
    private bool _isTouchWall;

    public bool IsInWallZone { get; private set; }

    private RaycastHit2D _lastWallHit;
    private bool _lastWallFacingRight;

    private StateMachineComp _stateMachine;

    public void Initialize(StateMachineComp machine)
    {
        _stateMachine = machine;
    }

    public void TickCheck(float dt)
    {
        CheckIsGrounded();
        CheckTouchWall();


        CheckWallZone();

        CheckStateChange();
    }


    private void CheckIsGrounded()
    {
        IsGrounded = false;
        var bounds = _stateMachine.Settings.Foot.bounds;
        var boxCastOrigin = new Vector2(bounds.center.x, bounds.min.y);
        var boxCastSize = new Vector2(bounds.size.x, _stateMachine.Settings.GroundDetectionRayLength);

        IsGrounded = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down,
            _stateMachine.Settings.GroundDetectionRayLength, 1 << LayerMask.NameToLayer("Ground")).collider != null;
    }

    private void CheckTouchWall()
    {
        IsTouchWall = false;

        var isFacingRight = _stateMachine.Turn.IsFacingRight;
        var bodyCollider = _stateMachine.Settings.Body;
        var endPoint = isFacingRight ? bodyCollider.bounds.max.x : bodyCollider.bounds.min.x;

        var boxCastOrigin = new Vector2(endPoint, bodyCollider.bounds.center.y);
        var boxCastSize = new Vector2(_stateMachine.Settings.WallDetectionRayLength, bodyCollider.bounds.size.y);

        var hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, _stateMachine.Settings.transform.right,
            _stateMachine.Settings.WallDetectionRayLength, 1 << LayerMask.NameToLayer("Ground"));

        if (hit.collider != null)
        {
            _lastWallHit = hit;
            _lastWallFacingRight = isFacingRight;
            IsTouchWall = true;
        }
    }

    private void CheckWallZone()
    {
        var isFacingRight = _stateMachine.Turn.IsFacingRight;
        if (_lastWallHit.collider == null || isFacingRight != _lastWallFacingRight)
        {
            IsInWallZone = false;
            _lastWallHit = new RaycastHit2D();
            return;
        }
        var body = _stateMachine.Settings.Body;
        var playerCenter = (Vector2)body.bounds.center;
        var wallPosition = _lastWallHit.point;

        float distanceToWall = Vector2.Distance(playerCenter, wallPosition);

        var hit = Physics2D.Raycast(playerCenter, _stateMachine.Settings.transform.right,
            distanceToWall * 10f, 1 << LayerMask.NameToLayer("Ground"));

        IsInWallZone = hit.collider != null && hit.collider == _lastWallHit.collider;

        if (_lastWallHit && !hit)
        {
            _lastWallHit = new RaycastHit2D();
        }
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