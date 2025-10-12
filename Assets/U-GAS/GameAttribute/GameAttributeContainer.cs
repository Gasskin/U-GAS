using System.Collections.Generic;

namespace U_GAS
{
    public class GameAttributeContainer
    {
        // 属性组
        private readonly Dictionary<EGameAttributeSet, BaseGameAttributeSet> _attributeSets = new();
        // 所有属性对应的修改器
        private readonly Dictionary<EGameAttribute, string> _attributeModifier = new();
        
        
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
                if (!_attributeModifier.ContainsKey(pair.Key))
                {
                    
                }
            }
        }
    }
}