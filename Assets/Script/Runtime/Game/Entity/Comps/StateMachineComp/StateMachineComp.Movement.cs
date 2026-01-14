using UnityEngine;

public class StateMachineMovement
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inVelocity">初速度</param>
    /// <param name="moveDirection">移动方向</param>
    /// <param name="speed">速度</param>
    /// <param name="acceleration">加速度</param>
    /// <param name="deceleration">减速度</param>
    /// <returns></returns>
    public float GetVelocityX(float inVelocityX, Vector2 moveDirection, float speed, 
        float acceleration, float deceleration)
    {
        var targetVelocity = moveDirection.x != 0 ? moveDirection.x * speed : 0;
        
        var smooth = moveDirection.x != 0
            ? acceleration
            : deceleration;

        return Mathf.Lerp(inVelocityX, targetVelocity, smooth * Time.fixedDeltaTime);
    }
}

public partial class StateMachineComp
{
    public StateMachineMovement Movement = new();
}