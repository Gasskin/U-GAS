using System;
using System.Collections.Generic;

namespace U_GAS
{
    public class GameAttributeComponent
    {
        private GameAbilityComponent _owner;

        // 属性组
        private readonly Dictionary<EGameAttribute, GameAttribute> _attributeDict = new();

        // 所有属性对应的修改器
        private readonly Dictionary<EGameAttribute, GameAttributeAggregator> _aggregatorDict = new();
        
        public void OnStart(GameAbilityComponent gameAbilityComponent)
        {
            _owner = gameAbilityComponent;
            foreach (var aggregator in _aggregatorDict)
                aggregator.Value.OnStart();
        }

        public void OnStop()
        {
            foreach (var aggregator in _aggregatorDict)
                aggregator.Value.OnStop();
        }


        public void AddAttribute(GameAttribute attr)
        {
            if (!_attributeDict.TryAdd(attr.Attribute, attr))
            {
                return;
            }
            var agg = new GameAttributeAggregator(attr, _owner);
            _aggregatorDict[attr.Attribute] = agg;
            agg.OnStart();
        }

        public float GetBaseValue(EGameAttribute eGameAttribute)
        {
            if (_attributeDict.TryGetValue(eGameAttribute, out var attr))
            {
                return attr.BaseValue;
            }
            return 0;
        }

        public float GetCurrentValue(EGameAttribute eGameAttribute)
        {
            if (_attributeDict.TryGetValue(eGameAttribute, out var attr))
            {
                return attr.CurrentValue;
            }
            return 0;
        }

        public GameAttribute GetAttribute(EGameAttribute eGameAttribute)
        {
            return _attributeDict.GetValueOrDefault(eGameAttribute, null);
        }

        public void DoSnapShot(Dictionary<EGameAttribute, float> snap)
        {
            snap.Clear();
            foreach (var attr in _attributeDict)
            {
                snap.Add(attr.Key, attr.Value.CurrentValue);
            }
        }
    }
}