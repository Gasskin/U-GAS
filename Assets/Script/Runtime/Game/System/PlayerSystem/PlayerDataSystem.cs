using System;
using System.Collections.Generic;
using cfg.Gas;
using Cysharp.Threading.Tasks;

public class PlayerDataSystem : BaseSystem
{
    private Entity _player;

    private int _level;

    public override async UniTask Initialize()
    {
        await UniTask.Yield();
    }

    public override void Destroy()
    {
        if (_player != null)
        {
            SystemDriver.EntitySystem?.DestroyEntity(_player);
        }
    }

    public async UniTask CreatePlayer()
    {
        // if (!SystemDriver.EntityRootSystem.TryGetHeroFirstCell(out var index, out var cell))
        // {
        //     return;
        // }

        _level = 10;

        var e = EntityHero.Create(new EntityHero()
        {
            Level = _level,
            HeroId = 1001,
        });
        await e.Initialize();

        await UniTask.Yield();
    }
}