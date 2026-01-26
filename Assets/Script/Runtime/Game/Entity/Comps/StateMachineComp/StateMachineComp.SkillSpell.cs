using cfg.Gas;

public partial class StateMachineComp
{
    public class StateMachineSkillSpell
    {
        public int SkillId { get; private set; }

        // 技能优先级
        public ESkillStagePriority SkillStagePriority { get; private set; }

        private StateMachineComp _stateMachine;

        private SkillSpellState _skillSpellState;

        public StateMachineSkillSpell(StateMachineComp stateMachine)
        {
            _stateMachine = stateMachine;
            _skillSpellState = _stateMachine._stateDic[typeof(SkillSpellState)] as SkillSpellState;
        }

        public void TrySpellSkill(int skillId)
        {
            switch (_stateMachine._curState)
            {
                case SkillSpellState:
                    if (SkillStagePriority == ESkillStagePriority.None) 
                    {
                        break;
                    }
                    if (SystemDriver.ConfigSystem.Tables.TbSkill.DataMap.TryGetValue(skillId, out var skill) &&
                        (skill.Priority == ESkillStagePriority.Force || skill.Priority > SkillStagePriority))
                    {
                        break;
                    }
                    return;
                case IdleState:
                case RunState:
                    break;
                default:
                    return;
            }
            // check cd
            // check cost
            SkillId = skillId;
            _stateMachine.ChangeState(_skillSpellState, true);
        }

        public void ChangeSkillStagePriority(ESkillStagePriority priority)
        {
            SkillStagePriority = priority;
        }
    }

    public StateMachineSkillSpell SkillSpell;
}