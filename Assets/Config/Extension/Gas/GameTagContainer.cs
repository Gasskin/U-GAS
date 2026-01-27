using System;
using System.Collections.Generic;
using Script.Runtime.Framework.System.GameTag;
using Script.Runtime.Game;

namespace cfg.Gas
{
    public class GameTagContainer
    {
        public List<EGameTag> Tags;

        private int[] _tags = new int[GameTagRegister.s_Size];

        public GameTagContainer(List<string> tags)
        {
            Tags = new();
            Array.Clear(_tags, 0, _tags.Length);
            foreach (var tag in tags)
            {
                Tags.Add(GameTagController.ToGameTag(tag));
            }
            foreach (var tag in Tags)
            {
                TravelAdd((int)tag);
            }
        }

        private void TravelAdd(int idx)
        {
            while (idx > 0)
            {
                if (idx >= _tags.Length)
                {
                    throw new ArgumentOutOfRangeException($"Tag index out of range");
                }
                _tags[idx] += 1;
                idx = GameTagRegister.s_Tree[idx];
            }
        }

        public bool HasTag(EGameTag tag)
        {
            return _tags[(int)tag] > 0;
        }

        public bool HasAnyTags(List<EGameTag> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (HasTag(tags[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}