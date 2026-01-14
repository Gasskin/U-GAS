using Sirenix.OdinInspector;
using UnityEngine;

public class StateMachineSetting: MonoBehaviour
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
    public float FallGravity=-1.5f;
    
    [FoldoutGroup("碰撞检测")]
    [LabelText("地面射线检测距离")]
    public float GroundDetectionRayLength;
}