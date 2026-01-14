using UnityEngine;

public partial class StateMachineComp
{
    public const string StateName_Idle = "idle";
    public const string StateName_Fall = "fall";
    public const string StateName_Jump = "jump";
    
    
    public Animator Animator { get;private set; }
}