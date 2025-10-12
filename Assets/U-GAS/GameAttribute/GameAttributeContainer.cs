using System.Collections.Generic;

namespace U_GAS
{
    public class GameAttributeContainer
    {
        private GameAbilityComponent _owner;
        
        // 属性组
        private readonly Dictionary<EGameAttributeSet, BaseGameAttributeSet> _attributeSets = new();

        // 所有属性对应的修改器
        private readonly Dictionary<EGameAttribute, GameAttributeCalculate> _attributeCalculate = new();

        public void OnStart(GameAbilityComponent gameAbilityComponent)
        {
            _owner = gameAbilityComponent;
            foreach (var aggregator in _attributeCalculate)
                aggregator.Value.OnStart();
        }

        public void OnStop()
        {
            foreach (var aggregator in _attributeCalculate)
                aggregator.Value.OnStop();
        }
        
        public void AddAttributeSet<T>(EGameAttributeSet setType)
        {
            if (_attributeSets.ContainsKey(setType))
            {
                return;
            }

            var set = GameAttributeSetRegister.New(setType);
            _attributeSets.Add(setType, set);

            foreach (var pair in set.attributes)
            {
                if (!_attributeCalculate.ContainsKey(pair.Key))
                {
                    var cal = new GameAttributeCalculate(pair.Value, _owner);
                    _attributeCalculate.Add(pair.Key, cal);
                    cal.OnStart();
                }
            }
        }
    }
}