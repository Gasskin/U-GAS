using UnityEngine;

public partial class StateMachineComp
{
    public const string STATE_NAME_IDLE = "idle";
    public const string STATE_NAME_FALL = "fall";
    
    
    public Animator Animator { get;private set; }
}