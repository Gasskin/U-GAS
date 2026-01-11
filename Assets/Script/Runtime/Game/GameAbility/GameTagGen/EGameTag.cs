using Sirenix.OdinInspector;
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
		/// "消耗"
		/// </summary>
		[LabelText("消耗")]
		Cost = 3,
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
	}
