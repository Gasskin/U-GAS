using System.Collections.Generic;
using Luban;
using MemoryPack;

namespace cfg.Gas
{
    public class TbSkillTimeline
    {
        public Dictionary<int, SkillTimeline> DataMap => _dataMap;
        public List<SkillTimeline> DataList => _dataList;

        public SkillTimeline GetOrDefault(int key) => _dataMap.GetValueOrDefault(key);
        public SkillTimeline Get(int key) => _dataMap[key];
        public SkillTimeline this[int key] => _dataMap[key];

        private readonly Dictionary<int, SkillTimeline> _dataMap;
        private readonly List<SkillTimeline> _dataList;

        public TbSkillTimeline(byte[] bytes)
        {
            _dataList = MemoryPackSerializer.Deserialize<List<SkillTimeline>>(bytes);
            _dataMap = new(_dataList.Count);
            foreach (var data in _dataList)
            {
                _dataMap.Add(data.Id, data);
            }
        }
    }
}