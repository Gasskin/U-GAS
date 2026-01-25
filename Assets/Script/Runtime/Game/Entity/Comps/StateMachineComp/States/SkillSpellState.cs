using System;
using System.Collections.Generic;

public class SkillSpellState: BaseState
{
    protected override List<Type> CheckToStates { get; } = new()
    {
        typeof(IdleState),
    };
    
    public override void OnEnter()
    {
    }

    public override void Tick(float dt)
    {
    }

    public override void FixedTick(float dt)
    {
    }

    public override void OnExit()
    {
    }

    protected override bool CanEnterTo(BaseState to)
    {
        switch (to)
        {
            case IdleState:
                return StateMachine.SkillSpell.IsSpell == false;
        }
        return false;
    }

    public override bool CanEnter()
    {
        return false;
    }

    public void PlayAnima(string anima)
    {
        StateMachine.PlayAnima(anima);
    }
}