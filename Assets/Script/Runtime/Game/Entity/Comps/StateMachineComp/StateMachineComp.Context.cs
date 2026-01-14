using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineContext
{
    public Vector2 MoveDir { get; set; }
    
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