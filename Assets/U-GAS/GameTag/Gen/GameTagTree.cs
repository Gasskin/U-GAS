namespace U_GAS
{
	public static class GameTagTree
	{
		public static readonly int Size = 18;
		public static readonly int[] Tree =
		{
			0, // 0 Null
			0, // 1 Ability
			1, // 2 Ability_Attack
			1, // 3 Ability_Defend
			1, // 4 Ability_Die
			1, // 5 Ability_Dodge
			1, // 6 Ability_Jump
			1, // 7 Ability_Move
			0, // 8 Faction
			8, // 9 Faction_Enemy
			8, // 10 Faction_Player
			0, // 11 State
			11, // 12 State_Buff
			12, // 13 State_Buff_Bulkup
			12, // 14 State_Buff_Defend
			11, // 15 State_Debuff
			15, // 16 State_Debuff_LoseBalance
			15, // 17 State_Debuff_Stun
		};
	}
}
