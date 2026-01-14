using Sirenix.OdinInspector;
using UnityEngine;

public class StateMachineSetting: MonoBehaviour
{
    [FoldoutGroup("组件索引")]
    [LabelText("主视图")]
    public SpriteRenderer Sprite;
    [FoldoutGroup("组件索引")]
    [LabelText("身体碰撞")]
    public Collider2D Body;
    [FoldoutGroup("组件索引")]
    [LabelText("脚步碰撞")]
    public Collider2D Foot;
    
    [FoldoutGroup("碰撞检测")]
    [LabelText("地面射线检测距离")]
    public float GroundDetectionRayLength;
}