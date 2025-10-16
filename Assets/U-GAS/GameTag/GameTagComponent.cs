using System;
using System.Collections.Generic;

namespace U_GAS
{
    public class GameTagComponent
    {
        public event Action OnTagIsDirty;

        private int[] _gameTag;
        private bool _isDirty;

        public void OnStart()
        {
            if (GameTagRegister.Size != GameTagRegister.Tree.Length)
            {
                throw new Exception("GameTag数据异常");
            }
            _gameTag = new int[GameTagRegister.Size];
            Array.Clear(_gameTag, 0, _gameTag.Length);
        }

        public void OnStop()
        {
            _gameTag = null;
        }

        public void AddTag(EGameTag gameTag)
        {
            TravelAdd(gameTag, 1);
        }

        public void AddTagsWithDirty(List<string> gameTags)
        {
            if (gameTags == null) 
            {
                return;
            }
            _isDirty = false;
            foreach (string gameTag in gameTags)
            {
                AddTag(GameTagRegister.String2Enum[gameTag]);
            }

            if (_isDirty)
            {
                OnTagIsDirty?.Invoke();
            }
        }

        public void RemoveTagsWithDirty(List<string> gameTags)
        {
            if (gameTags == null) 
            {
                return;
            }
            _isDirty = false;
            foreach (string gameTag in gameTags)
            {
                RemoveTag(GameTagRegister.String2Enum[gameTag]);
            }

            if (_isDirty)
            {
                OnTagIsDirty?.Invoke();
            }
        }

        public void RemoveTag(EGameTag gameTag)
        {
            TravelAdd(gameTag, -1);
        }

        public bool HasTag(EGameTag gameTag)
        {
            return _gameTag[(int)gameTag] > 0;
        }

        public bool HasAllTag(List<EGameTag> gameTags)
        {
            foreach (EGameTag gameTag in gameTags)
            {
                if (!HasTag(gameTag))
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasAllTag(List<string> gameTags)
        {
            foreach (string gameTag in gameTags)
            {
                var tag = GameTagRegister.String2Enum[gameTag];
                if (!HasTag(tag))
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasNoTags(List<EGameTag> gameTags)
        {
            foreach (EGameTag gameTag in gameTags)
            {
                if (HasTag(gameTag))
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasNoTags(List<string> gameTags)
        {
            foreach (string gameTag in gameTags)
            {
                var tag = GameTagRegister.String2Enum[gameTag];
                if (HasTag(tag))
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasAnyTags(List<EGameTag> gameTags)
        {
            foreach (EGameTag gameTag in gameTags)
            {
                if (HasTag(gameTag))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasAnyTags(List<string> gameTags)
        {
            foreach (string gameTag in gameTags)
            {
                var tag = GameTagRegister.String2Enum[gameTag];
                if (HasTag(tag))
                {
                    return true;
                }
            }
            return false;
        }

        private void TravelAdd(EGameTag tag, int value)
        {
            var idx = (int)tag;
            var oldValue = _gameTag[idx];
            _gameTag[idx] += value;
            _gameTag[idx] = Math.Clamp(_gameTag[idx], 0, int.MaxValue);
            var newValue = _gameTag[idx];
            _isDirty = _isDirty || ((oldValue == 0) != (newValue == 0));
            if (GameTagRegister.Tree[idx] > 0)
            {
                TravelAdd((EGameTag)GameTagRegister.Tree[idx], value);
            }
        }
    }
}