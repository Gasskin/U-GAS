using System;
using System.Collections.Generic;

namespace cfg.Gas
{
    public class GameTagContainer
    {
        public List<EGameTag> Tags;

        private int[] _tags = new int[GameTagRegister.Size];

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
                idx = GameTagRegister.Tree[idx];
            }
        }
    }
}