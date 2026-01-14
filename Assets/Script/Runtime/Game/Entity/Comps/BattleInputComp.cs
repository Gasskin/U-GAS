using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleInputComp : EntityComp
{
    public override int Priority => BATTLE_INPUT;

    private StateMachineComp stateMachine;
    
    public override async UniTask Initialize()
    {
        Entity.HasComp(STATE_MACHINE, out stateMachine);
        
        var sys = SystemDriver.InputSystem;
        var map = sys.GetMap<PlayerMap>();
        map.AddTrigger(map.Move, EInputType.Value, OnMove);
        
        await UniTask.Yield();
    }

    public override void Destroy()
    {
        var sys = SystemDriver.InputSystem;
        if (sys != null) 
        {
            var map = sys.GetMap<PlayerMap>();
            map.RemoveTrigger(map.Move, EInputType.Value, OnMove);
        }
    }

    private void OnMove(InputAction.CallbackContext ctx, InteractionParam param)
    {
        stateMachine.Context.MoveDir = ctx.ReadValue<Vector2>();
    }
}