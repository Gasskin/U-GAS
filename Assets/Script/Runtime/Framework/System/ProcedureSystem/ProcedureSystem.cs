using System.Collections.Generic;
using cfg.Gas;
using Cysharp.Threading.Tasks;

public class ProcedureSystem : BaseSystem
{
    public override async UniTask Initialize()
    {
        var e = SystemDriver.EntitySystem.CreateEntity();
        var gas = e.AddComp(new GasComp(new Dictionary<EAttributeId, float>()
        {
            { EAttributeId.HpBase, 100 },
            { EAttributeId.HpMult, 2 },
            { EAttributeId.HpAdd, 50 }
        }));
        gas.GameTagController.AddTag(EGameTag.Basic);
        gas.GameTagController.AddTag(EGameTag.Buff);
        gas.GameTagController.AddTag(EGameTag.Recover);
        gas.GameTagController.AddTag(EGameTag.Skill);

        e.AddComp(new ViewComp("Assets/Bundles/Prefabs/Unit/Hero.prefab"));

        var run = new RunState();
        var runJump = new RunJumpState();
        var runFall = new RunFallState();
        var jump = new JumpState();
        var idle = new IdleState();
        var fall = new FallState();
        var dash = new DashState();
        var dashFall = new DashFallState();

        idle.ToState.Add(fall);
        
        fall.ToState.Add(idle);

        e.AddComp(new StateMachineComp(run, runJump, runFall, jump, idle, fall, dash, dashFall));
        e.AddComp(new BattleInputComp());

        e.Initialize().Forget();

        await UniTask.Yield();
    }

    public override void Destroy()
    {
    }
}