using Sirenix.OdinInspector;
using UnityEngine;

public class StateMachineSetting : MonoBehaviour
{
    [FoldoutGroup("组件索引")]
    public SpriteRenderer Sprite;

    [FoldoutGroup("组件索引")]
    public Collider2D Body;

    [FoldoutGroup("组件索引")]
    public Collider2D Foot;

    [FoldoutGroup("组件索引")]
    public Rigidbody2D Rg;

    [FoldoutGroup("Fall")]
    [LabelText("地面重力")]
    public float GroundGravity = -1.5f;

    [FoldoutGroup("Fall")]
    [LabelText("最大下落速度")]
    public float MaxFallSpeed = -30f;

    [FoldoutGroup("Fall")]
    [LabelText("最大上升速度")]
    public float MaxUpwardSpeed = 50f;

    [FoldoutGroup("Fall")]
    [LabelText("重力缩放 - 按住跳跃时")]
    public float HoldJumpGravityMultiplayer = 1f;

    [FoldoutGroup("Fall")]
    [LabelText("重力缩放 - 普通掉落")]
    public float FallGravityMultiplayer = 1.2f;

    [FoldoutGroup("Fall")]
    [LabelText("重力缩放 - 跳跃最高点悬挂")]
    public float HangJumpGravityMultiplayer = 0.5f;

    [FoldoutGroup("Fall")]
    [LabelText("重力缩放 - 悬挂时速度要求")]
    public float HangJumpVelocity = 1f;


    [FoldoutGroup("Jump")]
    [LabelText("连续跳跃次数")]
    public int CanJumpCount = 2;

    [LabelText("最大跳跃高度")]
    public float MaxJumpHeight = 3.5f;

    [LabelText("最小跳跃高度")]
    public float MinJumpHeight = 1f;

    [LabelText("达到目标高度所需时间")]
    public float JumpToHeightTime = 0.3f;

    [FoldoutGroup("碰撞检测")]
    [LabelText("地面射线检测距离")]
    public float GroundDetectionRayLength;


    // v1 = v0 - g * t
    // 末速度为0
    // v0 = g * t
    // h = v0 * t - 0.5 * g * t²
    //   = g * t² - 0.5 * g * t²
    //   = 0.5 * g * t²
    // g = 2 * h / t²
    public float Gravity => 2f * MaxJumpHeight / Mathf.Pow(JumpToHeightTime, 2f);

    // v0 = g * t
    public float MaxJumpVelocity => Gravity * JumpToHeightTime;

    // v1² - v0² = 2 * a * t
    // 末速度为0，加速度为-g
    // - v0² = - 2 * g * t
    // v0 = sqrt(2 * g * t)
    public float MinJumpVelocity => Mathf.Sqrt(2f * Gravity * JumpToHeightTime);
}