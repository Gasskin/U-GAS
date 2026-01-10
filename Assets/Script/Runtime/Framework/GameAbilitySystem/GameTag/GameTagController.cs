using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTagController
{
#region static
    public static EGameTag ToGameTag(string gameTag)
    {
        var tag = GameTagRegister.StringToEnum.GetValueOrDefault(gameTag, EGameTag.None);
        if (tag == EGameTag.None)
        {
            throw new ArgumentOutOfRangeException($"不存在的GameTag：{gameTag}");
        }
        return EGameTag.None;
    }
#endregion


    private bool _isDirty;

    private int[] _tags = new int[GameTagRegister.Size];

    private GasComp _owner;


    public void Init(GasComp owner)
    {
        _owner = owner;
    }
    
    public void AddTag(EGameTag eTag)
    {
        TravelAdd((int)eTag, 1);
    }
    
    public void AddTagsWithDirty(List<EGameTag> grantedTags)
    {
        if (grantedTags == null)
        {
            return;
        }
        _isDirty = false;
        foreach (var tag in grantedTags)
        {
            AddTag(tag);
        }
        if (_isDirty)
        {
            // todo
            // _owner.GameEffectComponent.OnTagDirty();
        }
    }
    
    public void RemoveTag(EGameTag eTag)
    {
        TravelAdd((int)eTag, -1);
    }
    
    public void RemoveTagsWithDirty(List<EGameTag> grantedTags)
    {
        if (grantedTags == null)
        {
            return;
        }
        _isDirty = false;
        foreach (var tag in grantedTags)
        {
            RemoveTag(tag);
        }
        if (_isDirty)
        {
            // todo
            // _owner.GameEffectComponent.OnTagDirty();
        }
    }

    public bool HasTag(EGameTag eTag)
    {
        if ((int)eTag >= _tags.Length)
        {
            throw new ArgumentOutOfRangeException($"Tag index out of range");
        }
        return _tags[(int)eTag] > 0;
    }
    
    public bool HasAllTag(List<EGameTag> eTags)
    {
        if (eTags == null)
        {
            return true;
        }
        for (var i = 0; i < eTags.Count; i++)
        {
            if (!HasTag(eTags[i]))
            {
                return false;
            }
        }
        return true;
    }
    
    public bool HasAnyTag(List<EGameTag> eTags)
    {
        if (eTags == null)
        {
            return true;
        }
        for (var i = 0; i < eTags.Count; i++)
        {
            if (HasTag(eTags[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool HasNoTag(List<EGameTag> eTags)
    {
        if (eTags == null)
        {
            return true;
        }
        for (var i = 0; i < eTags.Count; i++)
        {
            if (HasTag(eTags[i]))
            {
                return false;
            }
        }
        return true;
    }
    
    private void TravelAdd(int idx, int value)
    {
        while (idx > 0)
        {
            if (idx >= _tags.Length)
            {
                throw new ArgumentOutOfRangeException($"Tag index out of range");
            }
            var oldValue = _tags[idx];
            _tags[idx] += value;
            _tags[idx] = Math.Max(_tags[idx], 0);
            var newValue = _tags[idx];
            _isDirty = _isDirty || ((oldValue == 0) != (newValue == 0));
            idx = GameTagRegister.Tree[idx];
        }
    }
}