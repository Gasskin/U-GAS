using System;
using MemoryPack;
using UnityEngine.InputSystem;

namespace cfg.Gas
{
    [Serializable]
    [MemoryPackable]
    public partial class AttackInputCacheClip : SkillTimelineClip
    {
        private bool _attack;

        protected override void OnStart()
        {
            _attack = false;
            SystemDriver.InputSystem.OnPlayerAttack += OnPlayerAttack;
        }


        protected override void OnEnd()
        {
            if (SystemDriver.EntitySystem.HasComp(Context.EntityId, EntityComp.Priority_BattleInput, out BattleInputComp input))
            {
                if (_attack)
                {
                    input.Attack();
                }
            }
            SystemDriver.InputSystem.OnPlayerAttack -= OnPlayerAttack;
        }


        public override void OnInterrupt()
        {
            SystemDriver.InputSystem.OnPlayerAttack -= OnPlayerAttack;
        }

        private void OnPlayerAttack(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                _attack = true;
            }
        }
    }
}