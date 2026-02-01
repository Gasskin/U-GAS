using System;
using System.Collections.Generic;
using cfg.Gas;
using Script.Runtime.Framework.System;

namespace Script.Runtime.Game.Entity
{
    public class SkillSpellState : BaseState
    {
        private SkillTimelineDriver _driver;

        protected override List<Type> CheckToStates { get; } = new()
        {
            typeof(RunState),
            typeof(IdleState),
        };

        public override void Initialize(StateMachineComp comp)
        {
            base.Initialize(comp);
            _driver = new(OnTimelineStart, null, null);
        }

        public override void OnEnter()
        {
            _driver.Start(new SkillTimelineContext()
            {
                EntityId = StateMachine.Entity.Id,
                SkillId = StateMachine.SkillSpell.SkillId,
            });
        }

        public override void Tick(float dt)
        {
            if (_driver.IsValid)
            {
                _driver.Tick(dt);
            }
        }

        public override void FixedTick(float dt)
        {
            if (_driver.IsValid)
            {
                _driver.FixedTick(dt);
            }
        }

        public override void OnExit()
        {
            StateMachine.SkillSpell.ChangeSkillStagePriority(ESkillStagePriority.None);
            if (_driver.IsValid)
            {
                _driver.Interrupt();
            }
        }

        protected override bool CanEnterTo(BaseState to)
        {
            if (_driver.IsValid)
            {
                return false;
            }
            switch (to)
            {
                case RunState:
                    return StateMachine.Context.MoveDir.x != 0;
                case IdleState:
                    return true;
            }
            return false;
        }

        public override bool CanEnter()
        {
            return false;
        }

        public void PlayAnima(string anima)
        {
            StateMachine.PlayAnima(anima);
        }

        private void OnTimelineStart()
        {
            if (SystemDriver.ConfigSystem.Tables.TbSkill.DataMap.TryGetValue(StateMachine.SkillSpell.SkillId,
                    out var skill))
            {
                StateMachine.SkillSpell.ChangeSkillStagePriority(skill.Priority);
            }
        }
    }
}