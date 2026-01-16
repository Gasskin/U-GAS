using System;
using System.Collections.Generic;
public class RunJumpState : BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
    };
    
    
    public override void Initialize(StateMachineComp comp)
    {
        base.Initialize(comp);
        StateMachine.Jump.OnMultiJump += () => { StateMachine.Play(StateMachineComp.StateName_MultiJump); };
    }

    public override void OnEnter()
    {
        StateMachine.Play(StateMachineComp.StateName_Jump);
    }

    public override void Tick(float dt)
    {
        StateMachine.Turn.CheckAndTurn();
        StateMachine.Jump.ReadInput();
    }

    public override void FixedTick(float dt)
    {
    }

    public override void OnExit()
    {
    }

    protected override bool CanEnterTo(BaseState to)
    {
        return false;
    }
    
    
    public override bool CanEnter()
    {
        return false;;
    }
}