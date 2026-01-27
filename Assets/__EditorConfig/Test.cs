using Script.Runtime.Framework.System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void TestButton()
    {
        SystemDriver.EntitySystem.HasEntity(1,out var entity);
        // entity.HasComp(EntityComp.Priority_SkillSpell, out SkillSpellComp skill);
        // skill.SpellSkill(1001);
    }
}
