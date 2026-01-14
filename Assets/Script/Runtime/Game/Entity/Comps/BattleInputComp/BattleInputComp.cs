using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

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
        _stateMachine.Context.MoveDir = ctx.ReadValue<Vector2>();
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