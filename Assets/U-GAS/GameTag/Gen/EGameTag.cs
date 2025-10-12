using Sirenix.OdinInspector;
namespace U_GAS
{
	public enum EGameTag
	{
		/// <summary>
		/// "能力"
		/// </summary>
		[LabelText("能力")]
		Ability = 1,
		/// <summary>
		/// "能力/攻击"
		/// </summary>
		[LabelText("能力/攻击")]
		Ability_Attack = 2,
		/// <summary>
		/// "能力/防御"
		/// </summary>
		[LabelText("能力/防御")]
		Ability_Defend = 3,
		/// <summary>
		/// "能力/死亡"
		/// </summary>
		[LabelText("能力/死亡")]
		Ability_Die = 4,
		/// <summary>
		/// "能力/闪避"
		/// </summary>
		[LabelText("能力/闪避")]
		Ability_Dodge = 5,
		/// <summary>
		/// "能力/跳跃"
		/// </summary>
		[LabelText("能力/跳跃")]
		Ability_Jump = 6,
		/// <summary>
		/// "能力/移动"
		/// </summary>
		[LabelText("能力/移动")]
		Ability_Move = 7,
		/// <summary>
		/// "固有标签"
		/// </summary>
		[LabelText("固有标签")]
		Faction = 8,
		/// <summary>
		/// "固有标签/敌人"
		/// </summary>
		[LabelText("固有标签/敌人")]
		Faction_Enemy = 9,
		/// <summary>
		/// "固有标签/玩家"
		/// </summary>
		[LabelText("固有标签/玩家")]
		Faction_Player = 10,
		/// <summary>
		/// "状态"
		/// </summary>
		[LabelText("状态")]
		State = 11,
		/// <summary>
		/// "状态/增益"
		/// </summary>
		[LabelText("状态/增益")]
		State_Buff = 12,
		/// <summary>
		/// "状态/增益/巨大化"
		/// </summary>
		[LabelText("状态/增益/巨大化")]
		State_Buff_Bulkup = 13,
		/// <summary>
		/// "状态/增益/防御"
		/// </summary>
		[LabelText("状态/增益/防御")]
		State_Buff_Defend = 14,
		/// <summary>
		/// "状态/减益"
		/// </summary>
		[LabelText("状态/减益")]
		State_Debuff = 15,
		/// <summary>
		/// "状态/减益/失衡"
		/// </summary>
		[LabelText("状态/减益/失衡")]
		State_Debuff_LoseBalance = 16,
		/// <summary>
		/// "状态/减益/眩晕"
		/// </summary>
		[LabelText("状态/减益/眩晕")]
		State_Debuff_Stun = 17,
	}
}
