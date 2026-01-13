using Sirenix.OdinInspector;
using UnityEngine;

public class StateMachineSetting: MonoBehaviour
{
    public Collider2D Body;
    public Collider2D Foot;
    
    [TitleGroup("碰撞检测")]
    [LabelText("地面射线检测距离")]
    public float GroundDetectionRayLength;
    
    
    
    void OnDrawGizmos() {
        if (Foot == null) return;

        var bounds = Foot.bounds;
        // 你的逻辑中的起点和尺寸
        var boxCastOrigin = new Vector2(bounds.center.x, bounds.min.y);
        var boxCastSize = new Vector2(bounds.size.x, GroundDetectionRayLength);
    
        // 绘制 BoxCast 的初始位置 (线框)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCastOrigin, boxCastSize);

        // 绘制 BoxCast 扫过的最终位置 (实心)
        Gizmos.color = Color.red;
        Vector2 endPos = boxCastOrigin + Vector2.down * GroundDetectionRayLength;
        Gizmos.DrawWireCube(endPos, boxCastSize);
    }
}