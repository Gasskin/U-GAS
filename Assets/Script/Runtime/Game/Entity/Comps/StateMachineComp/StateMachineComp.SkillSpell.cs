public partial class StateMachineComp
{
    public class StateMachineSkillSpell
    {
        public int SkillId { get; private set; }
        
        private StateMachineComp _stateMachine;

        public StateMachineSkillSpell(StateMachineComp stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void TrySpellSkill(int skillId)
        {
            if (_stateMachine._curState is not IdleState &&
                _stateMachine._curState is not RunState)
            {
                return;
            }
            // check cd
            // check cost
            SkillId = skillId;
            _stateMachine.ChangeState<SkillSpellState>();
        }
    }

    public StateMachineSkillSpell SkillSpell;
}