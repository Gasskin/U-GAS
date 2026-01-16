using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineContext
{
    // public Vector2Int MoveDir { get; set; }
    public int MoveDir { get; set; }
    
    // jump
    public ButtonState Jump { get; set; } = new();

    public void LateTick(float dt)
    {
        Jump.ResetFrameState();
    }
}

public partial class StateMachineComp
{
    public StateMachineContext Context = new();
}