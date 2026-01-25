using System;
using System.Collections.Generic;
using cfg.Gas;
using Cysharp.Threading.Tasks;
using UI;

public class ProcedureSystem : BaseSystem, ITickSystem
{
    private BaseProcedure _nowProcedure;

    private Dictionary<Type, BaseProcedure> _procedures = new();
    
    public override async UniTask Initialize()
    {
        Register(new BattleProcedure());

        await UniTask.Yield();
    }
    
    public void Tick(float dt)
    {
        _nowProcedure?.Tick(dt);
    }

    public override void Destroy()
    {
        _nowProcedure?.Exit();
    }

    public void ChangeProcedure<T>() where T : BaseProcedure, new()
    {
        var type = typeof(T);
        _nowProcedure?.Exit();
        _nowProcedure = null;
        if (_procedures.TryGetValue(type, out var procedure))
        {
            _nowProcedure = procedure;
            _nowProcedure?.Enter();
        }
    } 

    private void Register(BaseProcedure procedure)
    {
        _procedures.Add(procedure.GetType(), procedure);
    }
}