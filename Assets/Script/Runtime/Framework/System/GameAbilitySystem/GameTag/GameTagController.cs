using System;
using System.Collections.Generic;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Game;
using Script.Runtime.Game.GameAbility;
using UnityEngine;

namespace Script.Runtime.Framework.System.GameTag
{
    public class TagTimer : IPoolObject
    {
        public EGameTag Tag;
        public float Duration;

        public void OnRelease()
        {
            Tag = EGameTag.None;
            Duration = 0f;
        }
    }

    public class GameTagController
    {
    #region static
        public static EGameTag ToGameTag(string gameTag)
        {
            var tag = GameTagRegister.s_StringToEnum.GetValueOrDefault(gameTag, EGameTag.None);
            if (tag == EGameTag.None)
            {
                Debug.LogError($"不存在的GameTag：{gameTag}");
            }
            return tag;
        }
    #endregion


        private bool _isDirty;

        private int[] _tags = new int[GameTagRegister.s_Size];

        private GasComp _owner;

        private LinkedList<TagTimer> _tagTimers = new();

        public void Init(GasComp owner)
        {
            _owner = owner;
        }

        public void Tick(float dt)
        {
            var node = _tagTimers.First;
            while (node != null)
            {
                var next = node.Next;
                node.Value.Duration -= dt;
                if (node.Value.Duration <= 0)
                {
                    RemoveTag(node.Value.Tag);
                    Pool<TagTimer>.Release(node.Value);
                    _tagTimers.Remove(node);
                }
                node = next;
            }
        }

        public void AddTag(EGameTag eTag)
        {
            TravelAdd((int)eTag, 1);
        }

        public void AddTagTimer(EGameTag tag, float duration)
        {
            var timer = Pool<TagTimer>.Get();
            timer.Tag = tag;
            timer.Duration = duration;
            AddTag(tag);
            _tagTimers.AddLast(timer);
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
                _owner.GameEffectController.OnTagDirty();
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
                _owner.GameEffectController.OnTagDirty();
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
                idx = GameTagRegister.s_Tree[idx];
            }
        }

#if UNITY_EDITOR
        public void GetAllTags(List<EGameTag> tags)
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                if (_tags[i] > 0)
                {
                    tags.Add((EGameTag)i);
                }
            }
        }
#endif
    }
}