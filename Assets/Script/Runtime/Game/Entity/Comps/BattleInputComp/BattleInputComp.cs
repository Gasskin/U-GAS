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

    private void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        var dir = 0;
        var input = ctx.ReadValue<Vector2>();
        if (!Mathf.Approximately(input.x, 0f))
        {
            dir = (int)Mathf.Sign(input.x);
        }
        _stateMachine.Context.MoveDir = dir;
    }

    private void OnPlayerJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _stateMachine.Context.Jump.Start();
        }
        else if (ctx.canceled)
        {
            _stateMachine.Context.Jump.Cancel();
        }
    }
}