using UnityEngine;

public class StateMachineContext
{
    public Vector2 MoveDir;
}

public partial class StateMachineComp
{
    public StateMachineContext Context = new();
}