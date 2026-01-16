using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class StateMachineSetting : MonoBehaviour
{
    [FoldoutGroup("动画组")]
    public List<AnimationClip> Clips = new();

    public Dictionary<string, AnimationClip> ClipDict = new();

    [FoldoutGroup("组件索引")]
    public SpriteRenderer Sprite;

    [FoldoutGroup("组件索引")]
    public Collider2D Body;

    [FoldoutGroup("组件索引")]
    public Collider2D Foot;

    [FoldoutGroup("组件索引")]
    public Rigidbody2D Rg;

#region Fall
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
#endregion

#region Jump
    [FoldoutGroup("Jump")]
    [LabelText("连续跳跃次数")]
    public int CanJumpCount = 2;

    [FoldoutGroup("Jump")]
    [LabelText("最大跳跃高度")]
    public float MaxJumpHeight = 3.5f;

    [FoldoutGroup("Jump")]
    [LabelText("最小跳跃高度")]
    public float MinJumpHeight = 1f;

    [FoldoutGroup("Jump")]
    [LabelText("达到目标高度所需时间")]
    public float JumpToHeightTime = 0.3f;
#endregion

#region Air
    [FoldoutGroup("Air")]
    [LabelText("空中速度")]
    public float AirSpeed = 100f;
    [FoldoutGroup("Air")]
    [LabelText("普通跳跃空中加速度")]
    public float JumpAirAcceleration = 100f;
    [FoldoutGroup("Air")]
    [LabelText("普通跳跃空中减速度")]
    public float JumpAirDeceleration = 5f;
    [FoldoutGroup("Air")]
    [LabelText("奔跑跳跃空中加速度")]
    public float RunJumpAirAcceleration = 150f;
    [FoldoutGroup("Air")]
    [LabelText("奔跑跳跃空中减速度")]
    public float RunJumpAirDeceleration = 23f;
#endregion

#region Run
    [FoldoutGroup("Run")]
    [LabelText("跑步速度")]
    public float RunSpeed = 15f;

    [FoldoutGroup("Run")]
    [LabelText("跑步加速度")]
    public float RunAcceleration = 150f;

    [FoldoutGroup("Run")]
    [LabelText("跑步减速度")]
    public float RunDeceleration = 150f;
#endregion

    
    [FoldoutGroup("碰撞检测")]
    [LabelText("地面射线检测距离")]
    public float GroundDetectionRayLength = 0.02f;

    [FoldoutGroup("碰撞检测")]
    [LabelText("墙面面射线检测距离")]
    public float WallDetectionRayLength = 0.02f;

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

    // v1² - v0² = 2 * a * s
    // 末速度为0，加速度为-g
    // - v0² = - 2 * g * h
    // v0 = sqrt(2 * g * h)
    public float MinJumpVelocity => Mathf.Sqrt(2f * Gravity * MinJumpHeight);


    private void Start()
    {
        ClipDict.Clear();
        foreach (var animationClip in Clips)
        {
            ClipDict.Add(animationClip.name, animationClip);
        }
    }
}