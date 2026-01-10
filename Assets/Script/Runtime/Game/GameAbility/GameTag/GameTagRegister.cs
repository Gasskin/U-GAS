using System;
using System.Collections.Generic;
	public static class GameTagRegister
	{
		public static readonly int Size = 11;
		public static readonly int[] Tree =
		{
			0,	// 0 Null
			0,	// 1 Buff
			0,	// 2 Cost
			2,	// 3 Cost_Test1
			0,	// 4 Damage
			0,	// 5 DeBuff
			0,	// 6 Recover
			0,	// 7 Skill
			7,	// 8 Skill_Hero1
			8,	// 9 Skill_Hero1_Attack1
			7,	// 10 Skill_Hero2
		};
		public static readonly Dictionary<string, EGameTag> StringToEnum = new()
		{
			{ "Buff", EGameTag.Buff },
			{ "Cost", EGameTag.Cost },
			{ "Damage", EGameTag.Damage },
			{ "DeBuff", EGameTag.DeBuff },
			{ "Recover", EGameTag.Recover },
			{ "Skill", EGameTag.Skill },
			{ "Cost_Test1", EGameTag.Cost_Test1 },
			{ "Skill_Hero1", EGameTag.Skill_Hero1 },
			{ "Skill_Hero2", EGameTag.Skill_Hero2 },
			{ "Skill_Hero1_Attack1", EGameTag.Skill_Hero1_Attack1 },
		};

#if UNITY_EDITOR
        static GameTagRegister()
        {
            if (Tree == null || Tree.Length != Size)
            {
                throw new Exception($"Tree.Length({Tree?.Length}) != Size({Size})");
            }
            for (int i = 0; i < Tree.Length; i++)
            {
                int p = Tree[i];
                if (p < -1 || p >= Size)
                {
                    throw new Exception($"Illegal parent index: {i} -> {p}");
                }
            }

            // 简单环检测
            var seen = new bool[Size];
            for (int i = 0; i < Size; i++)
            {
                Array.Clear(seen, 0, seen.Length);
                int cur = i;
                while (cur > 0)
                {
                    if (seen[cur])
                    {
                        throw new Exception($"Cycle detected at {i} (via {cur})");
                    }
                    seen[cur] = true;
                    cur = Tree[cur];
                }
            }
        }
#endif
	}
