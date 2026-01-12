using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming
	public static class GameTagRegister
	{
		public static readonly int s_Size = 8;
		public static readonly int[] s_Tree =
		{
			0,	// 0 Null
			0,	// 1 Basic
			0,	// 2 Buff
			0,	// 3 Cost
			0,	// 4 Damage
			0,	// 5 DeBuff
			0,	// 6 Recover
			0,	// 7 Skill
		};
		public static readonly Dictionary<string, EGameTag> s_StringToEnum = new()
		{
			{ "Basic", EGameTag.Basic },
			{ "Buff", EGameTag.Buff },
			{ "Cost", EGameTag.Cost },
			{ "Damage", EGameTag.Damage },
			{ "DeBuff", EGameTag.DeBuff },
			{ "Recover", EGameTag.Recover },
			{ "Skill", EGameTag.Skill },
		};

#if UNITY_EDITOR
        static GameTagRegister()
        {
            if (s_Tree == null || s_Tree.Length != s_Size)
            {
                throw new Exception($"s_Tree.Length({s_Tree?.Length}) != s_Size({s_Size})");
            }
            for (int i = 0; i < s_Tree.Length; i++)
            {
                int p = s_Tree[i];
                if (p < -1 || p >= s_Size)
                {
                    throw new Exception($"Illegal parent index: {i} -> {p}");
                }
            }

            // 简单环检测
            var seen = new bool[s_Size];
            for (int i = 0; i < s_Size; i++)
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
                    cur = s_Tree[cur];
                }
            }
        }
#endif
	}
