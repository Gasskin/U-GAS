using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public struct SavedInput
{
    public Vector2 Vector2;
    public bool Started;
    public bool Canceled;
}

public class BattleInputComp : EntityComp
{
    public override int Priority => BATTLE_INPUT;

    private StateMachineComp _stateMachine;

    public override bool NeedFixedTick => true;

    private List<SavedInput> _savedPlayerMove = new(32);
    private List<SavedInput> _savedPlayerJump = new(32);

    public override async UniTask Initialize()
    {
        Entity.HasComp(STATE_MACHINE, out _stateMachine);

        var input = SystemDriver.InputSystem;
        input.OnPlayerMove += OnPlayerMove;
        input.OnPlayerJump += OnPlayerJump;
        
        await UniTask.Yield();
    }

    public override void Destroy()
    {
        var input = SystemDriver.InputSystem;
        if (input == null) 
        {
            return;
        }
        input.OnPlayerMove -= OnPlayerMove;
        input.OnPlayerJump -= OnPlayerJump;
    }

    public override void FixedTick(float dt)
    {
        for (int i = 0; i < _savedPlayerMove.Count; i++)
        {
            _stateMachine.Context.MoveDir = _savedPlayerMove[i].Vector2;
        }
        _savedPlayerMove.Clear();
        
        for (int i = 0; i < _savedPlayerJump.Count; i++)
        {
            var save = _savedPlayerJump[i];
            if (save.Started)
            {
                _stateMachine.Context.Jump.Start();
            }
            else if (save.Canceled)
            {
                _stateMachine.Context.Jump.Cancel();
            }
        }
        _savedPlayerJump.Clear();
    }

    private void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        _savedPlayerMove.Add(new SavedInput()
        {
            Vector2 = ctx.ReadValue<Vector2>()
        });
    }
    
    private void OnPlayerJump(InputAction.CallbackContext ctx)
    {
        _savedPlayerJump.Add(new SavedInput()
        {
            Started = ctx.started,
            Canceled = ctx.canceled
        });
    }
}