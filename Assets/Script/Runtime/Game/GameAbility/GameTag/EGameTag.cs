using Sirenix.OdinInspector;
// ReSharper disable InconsistentNaming
public enum EGameTag
	{
		None = 0,
		/// <summary>
		/// "增益效果"
		/// </summary>
		[LabelText("增益效果")]
		Buff = 1,
		/// <summary>
		/// "消耗"
		/// </summary>
		[LabelText("消耗")]
		Cost = 2,
		/// <summary>
		/// "消耗/测试"
		/// </summary>
		[LabelText("消耗/测试")]
		Cost_Test1 = 3,
		/// <summary>
		/// "伤害"
		/// </summary>
		[LabelText("伤害")]
		Damage = 4,
		/// <summary>
		/// "减益效果"
		/// </summary>
		[LabelText("减益效果")]
		DeBuff = 5,
		/// <summary>
		/// "恢复"
		/// </summary>
		[LabelText("恢复")]
		Recover = 6,
		/// <summary>
		/// "技能"
		/// </summary>
		[LabelText("技能")]
		Skill = 7,
		/// <summary>
		/// "技能/英雄1"
		/// </summary>
		[LabelText("技能/英雄1")]
		Skill_Hero1 = 8,
		/// <summary>
		/// "技能/英雄1/攻击1"
		/// </summary>
		[LabelText("技能/英雄1/攻击1")]
		Skill_Hero1_Attack1 = 9,
		/// <summary>
		/// "技能/英雄2"
		/// </summary>
		[LabelText("技能/英雄2")]
		Skill_Hero2 = 10,
	}
