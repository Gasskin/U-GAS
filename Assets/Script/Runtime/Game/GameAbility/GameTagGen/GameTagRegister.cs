using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming
	namespace Script.Runtime.Game.GameAbility
    {
        public static class GameTagRegister
        {
            public static readonly int s_Size = 14;
            public static readonly int[] s_Tree =
            {
                0,	// 0 Null
                0,	// 1 Basic
                0,	// 2 Buff
                0,	// 3 Combo
                3,	// 4 Combo_C1
                3,	// 5 Combo_C2
                3,	// 6 Combo_C3
                3,	// 7 Combo_C4
                3,	// 8 Combo_C5
                0,	// 9 Cost
                0,	// 10 Damage
                0,	// 11 DeBuff
                0,	// 12 Recover
                0,	// 13 Skill
            };
            public static readonly Dictionary<string, EGameTag> s_StringToEnum = new()
            {
                { "Basic", EGameTag.Basic },
                { "Buff", EGameTag.Buff },
                { "Combo", EGameTag.Combo },
                { "Cost", EGameTag.Cost },
                { "Damage", EGameTag.Damage },
                { "DeBuff", EGameTag.DeBuff },
                { "Recover", EGameTag.Recover },
                { "Skill", EGameTag.Skill },
                { "Combo_C1", EGameTag.Combo_C1 },
                { "Combo_C2", EGameTag.Combo_C2 },
                { "Combo_C3", EGameTag.Combo_C3 },
                { "Combo_C4", EGameTag.Combo_C4 },
                { "Combo_C5", EGameTag.Combo_C5 },
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
    }
