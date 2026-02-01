using System;
using System.Collections.Generic;
using MemoryPack;
using Script.Runtime.Game;
using Script.Runtime.Game.System.GameAbilitySystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace cfg.Gas
{
    [Serializable]
    [MemoryPackable]
    public partial class StringGameTag
    {
        [ValueDropdown("GetGameTagsName")]
        [SerializeField]
        [MemoryPackInclude]
        private string _tagName;

        private int _enumValue = -1;

        public EGameTag Tag
        {
            get
            {
                if (_enumValue == -1)
                {
                    _enumValue = (int)GameTagRegister.s_StringToEnum.GetValueOrDefault(_tagName, EGameTag.None);
                }
                return (EGameTag)_enumValue;
            }
        }

        public static implicit operator EGameTag(StringGameTag tag)
        {
            return tag.Tag;
        }        
#if UNITY_EDITOR
        private static List<string> _gameTags;

        private List<string> GetGameTagsName()
        {
            if (_gameTags == null)
            {
                _gameTags = new List<string>();
                _gameTags.AddRange(Enum.GetNames(typeof(EGameTag)));
            }
            return _gameTags;
        }
#endif
    }
}