using cfg.Gas;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;

namespace Script.Runtime.Game
{
    public class SkillSpellComp : EntityComp
    {
        public override int Priority => Priority_SkillSpell;
        public override bool NeedTick => true;

        private SkillTimelineDriver _skillTimelineDriver;

        public override async UniTask Initialize()
        {
            _skillTimelineDriver = new(OnTimelineStart, OnTimelineInterrupt, OnTimelineEnd);
            await UniTask.Yield();
        }

        public override void Tick(float dt)
        {
            if (!_skillTimelineDriver.IsValid)
            {
                return;
            }
            _skillTimelineDriver.Tick(dt);
        }

        public void SpellSkill(int skillId)
        {
            // todo 检查消耗等等

            StartTimelineDriver(skillId);
        }

        public void InterruptSkill()
        {
            if (!_skillTimelineDriver.IsValid)
            {
                return;
            }
            _skillTimelineDriver.Interrupt();
        }


        private void StartTimelineDriver(int skillId)
        {
            _skillTimelineDriver.Start(new SkillTimelineContext()
            {
                EntityId = Entity.Id,
                SkillId = skillId
            });
        }

        private void OnTimelineStart()
        {
   
        }

        private void OnTimelineInterrupt()
        {
       
        }

        private void OnTimelineEnd()
        {
      
        }
    }
}