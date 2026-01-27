using cfg.Gas;
using Script.Runtime.Framework.System;
using UnityEngine;

namespace Script.Runtime.Game
{
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

            public bool TrySpellSkill(int skillId)
            {
                if (!SystemDriver.ConfigSystem.Tables.TbSkill.DataMap.TryGetValue(skillId, out var skill))
                {
                    Debug.LogError($"不存在技能配置：{skillId}");
                    return false;
                }

                if (!_stateMachine.Entity.HasComp(Priority_Gas, out GasComp owner))
                {
                    Debug.LogError($"不存在GasComp：{_stateMachine.Entity}");
                    return false;
                }

                // check can enter
                if (!CheckStateCanEnter(skill))
                {
                    return false;
                }

                // check transform
                if (CheckSkillTransform(skill, owner))
                {
                    return true;
                }
            
                // check cd
                // check cost
            
                SkillId = skillId;
                _stateMachine.ChangeState(_skillSpellState, true);
                return true;
            }

            public void ChangeSkillStagePriority(ESkillStagePriority priority)
            {
                SkillStagePriority = priority;
            }

            private bool CheckStateCanEnter(Skill skill)
            {
                switch (_stateMachine._curState)
                {
                    case SkillSpellState:
                        if (SkillStagePriority == ESkillStagePriority.None)
                        {
                            break;
                        }
                        if (skill.Priority == ESkillStagePriority.Force || skill.Priority > SkillStagePriority)
                        {
                            break;
                        }
                        return false;
                    case IdleState:
                    case RunState:
                        break;
                    default:
                        return false;
                }
                return true;
            }

            private bool CheckSkillTransform(Skill skill, GasComp owner)
            {
                for (int i = 0; i < skill.Transform_Ref.Count; i++)
                {
                    var transform = skill.Transform_Ref[i];
                    if (transform.Condition is ICheckCondition check && check.Check(owner))
                    {
                        if (TrySpellSkill(transform.ToSkillId))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public StateMachineSkillSpell SkillSpell;
    }
}