using System;
using System.Collections.Generic;
using cfg.Gas;

public class GameAttributeController
{
    private Dictionary<EAttributeId, GameAttribute> _attributes = new();
    private GasComp _owner;

    public void Init(GasComp owner, Dictionary<EAttributeId, float> initAttr)
    {
        _owner = owner;
        _attributes.Clear();
        var attrs = (EAttributeId[])Enum.GetValues(typeof(EAttributeId));
        foreach (var attr in attrs)
        {
            _attributes.Add(attr, new GameAttribute(_owner, attr));
        }
        if (initAttr != null)
        {
            foreach (var pair in initAttr)
            {
                if (_attributes.TryGetValue(pair.Key, out GameAttribute attr))
                {
                    attr.InitValue(pair.Value);
                }
            }
        }

        // 添加相对属性
        _owner.ApplyGameEffectTo(1, _owner);
        _owner.ApplyGameEffectTo(2, _owner);
        _owner.ApplyGameEffectTo(3, _owner);
        _owner.ApplyGameEffectTo(4, _owner);
        _owner.ApplyGameEffectTo(5, _owner);
        _owner.ApplyGameEffectTo(6, _owner);
        _owner.ApplyGameEffectTo(7, _owner);
        _owner.ApplyGameEffectTo(8, _owner);

        // 重置血量
        _attributes[EAttributeId.HpNow].InitValue(_attributes[EAttributeId.HpMax].CurrentValue);
    }

    public void OnGameEffectDirty()
    {
        foreach (var attr in _attributes.Values)
        {
            attr.OnGameEffectDirty();
        }
    }
    
    public float GetCurrentValue(EAttributeId attributeId)
    {
        return _attributes[attributeId].CurrentValue;
    }

    public float GetBaseValue(EAttributeId attributeId)
    {
        return _attributes[attributeId].BaseValue;
    }

    public GameAttribute GetAttribute(EAttributeId attr)
    {
        return _attributes.GetValueOrDefault(attr, null);
    }

#if UNITY_EDITOR
    public void GetAllAttributes(List<GameAttribute> attributes)
    {
        attributes.Clear();
        attributes.AddRange(_attributes.Values);
    }
#endif
}