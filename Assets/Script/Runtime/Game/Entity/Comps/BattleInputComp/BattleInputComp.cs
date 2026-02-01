using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Runtime.Game.Entity
{
    [EntityCompPriority(Priority_BattleInput)]
    public class BattleInputComp : EntityComp
    {

        private StateMachineComp _stateMachine;


        public override async UniTask Initialize()
        {
            Entity.HasComp(out _stateMachine);

            var input = SystemDriver.InputSystem;
            input.OnPlayerMove += OnPlayerMove;
            input.OnPlayerJump += OnPlayerJump;
            input.OnPlayerDash += OnPlayerDash;
            input.OnPlayerAttack += OnPlayerAttack;

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
            input.OnPlayerDash -= OnPlayerDash;
            input.OnPlayerAttack -= OnPlayerAttack;
        }

        private void OnPlayerMove(InputAction.CallbackContext ctx)
        {
            var dirX = 0;
            var dirY = 0;
            var input = ctx.ReadValue<Vector2>();
            if (!Mathf.Approximately(input.x, 0f))
            {
                dirX = (int)Mathf.Sign(input.x);
            }
            if (!Mathf.Approximately(input.y, 0f))
            {
                dirY = (int)Mathf.Sign(input.y);
            }
            _stateMachine.Context.MoveDir = new Vector2Int(dirX, dirY);
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

        private void OnPlayerDash(InputAction.CallbackContext ctx)
        {
            // if (ctx.started)
            // {
            //     _stateMachine.Context.Dash.Start();
            // }
            // else if (ctx.canceled)
            // {
            //     _stateMachine.Context.Dash.Cancel();
            // }
            _stateMachine.SkillSpell.TrySpellSkill(1001201);
        }


        private void OnPlayerAttack(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                Attack();
            }
        }

        public void Attack()
        {
            _stateMachine.SkillSpell.TrySpellSkill(1001101);
        }
    }
}