using System.Collections.Generic;
using cfg.Gas;
using Cysharp.Threading.Tasks;

public class ProcedureSystem : BaseSystem
{
    public override async UniTask Initialize()
    {
        var sys = SystemDriver.GetSystem<EntitySystem>();
        var e = sys.CreateEntity();
        var gas = e.AddComp(new GasComp());
        gas.Init(new Dictionary<EAttributeId, float>()
        {
            { EAttributeId.HpBase, 100 },
            { EAttributeId.HpMult, 2 },
            { EAttributeId.HpAdd, 50 }
        });
        gas.GameTagController.AddTag(EGameTag.Basic);
        gas.GameTagController.AddTag(EGameTag.Buff);
        gas.GameTagController.AddTag(EGameTag.Recover);
        gas.GameTagController.AddTag(EGameTag.Skill);

        e.AddComp(new EntityViewComp("Assets/Bundles/Prefabs/Unit/Hero.prefab"));
        
        await UniTask.Yield();
    }

    public override void Close()
    {
    }
}