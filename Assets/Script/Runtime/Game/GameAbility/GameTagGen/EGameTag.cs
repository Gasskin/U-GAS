using Sirenix.OdinInspector;

namespace Script.Runtime.Game
{
    public enum EGameTag
    {
        None = 0,
        /// <summary>
        /// "基础效果"
        /// </summary>
        [LabelText("基础效果")]
        Basic = 1,
        /// <summary>
        /// "增益效果"
        /// </summary>
        [LabelText("增益效果")]
        Buff = 2,
        /// <summary>
        /// "连招"
        /// </summary>
        [LabelText("连招")]
        Combo = 3,
        /// <summary>
        /// "连招/一段标记"
        /// </summary>
        [LabelText("连招/一段标记")]
        Combo_C1 = 4,
        /// <summary>
        /// "连招/二段标记"
        /// </summary>
        [LabelText("连招/二段标记")]
        Combo_C2 = 5,
        /// <summary>
        /// "连招/三段标记"
        /// </summary>
        [LabelText("连招/三段标记")]
        Combo_C3 = 6,
        /// <summary>
        /// "连招/四段标记"
        /// </summary>
        [LabelText("连招/四段标记")]
        Combo_C4 = 7,
        /// <summary>
        /// "连招/五段标记"
        /// </summary>
        [LabelText("连招/五段标记")]
        Combo_C5 = 8,
        /// <summary>
        /// "消耗"
        /// </summary>
        [LabelText("消耗")]
        Cost = 9,
        /// <summary>
        /// "伤害"
        /// </summary>
        [LabelText("伤害")]
        Damage = 10,
        /// <summary>
        /// "减益效果"
        /// </summary>
        [LabelText("减益效果")]
        DeBuff = 11,
        /// <summary>
        /// "恢复"
        /// </summary>
        [LabelText("恢复")]
        Recover = 12,
        /// <summary>
        /// "技能"
        /// </summary>
        [LabelText("技能")]
        Skill = 13,
    }
}
