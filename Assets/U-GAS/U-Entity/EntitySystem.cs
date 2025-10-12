using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace U_GAS
{
    public static class EntitySystemExtension
    {
        public static void AddComponent(this int idx, BaseEntityComponent component)
        {
            EntitySystem.Instance.AddComponent(idx, component);
        }
    }

    public class EntitySystem : MonoBehaviour
    {
        public static EntitySystem Instance;

        private const int _DEFAULT_SIZE = 10000;
        private const int _EXPAND_SIZE = 100;

        private int _nowSize;
        private HashSet<int> _usingIndex = new();
        private Queue<int> _emptyIndex = new();

        private Dictionary<Type, List<BaseEntityComponent>> _typeToComponents = new();
        private readonly Dictionary<Type, HashSet<int>> _activeComponentIndex = new();

        private List<Type> _updateType = new(100);
        private List<int> _updateIndex = new(1000);

        private void Awake()
        {
            _nowSize = _DEFAULT_SIZE;

            for (int i = 0; i < _nowSize; i++)
            {
                _emptyIndex.Enqueue(i);
            }

            var baseType = typeof(BaseEntityComponent);
            var types = baseType.Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t != baseType && baseType.IsAssignableFrom(t));
            foreach (var type in types)
            {
                var list = new List<BaseEntityComponent>(_nowSize);
                for (int i = 0; i < _nowSize; i++)
                {
                    list.Add(null);
                }
                _typeToComponents.Add(type, list);
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            // 这里只要Stop就行，Destroy了不用管其他逻辑
            foreach (var idx in _usingIndex)
            {
                foreach (var comps in _typeToComponents.Values)
                {
                    comps[idx]?.Stop();
                }
            }
            Instance = null;
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            _updateType.Clear();
            foreach (var pair in _activeComponentIndex)
            {
                _updateType.Add(pair.Key);
            }

            for (int i = 0; i < _updateType.Count; i++)
            {
                var type = _updateType[i];
                _updateIndex.Clear();
                if (_activeComponentIndex.TryGetValue(type, out var actives))
                {
                    _updateIndex.AddRange(actives);
                }

                if (_typeToComponents.TryGetValue(type, out var list))
                {
                    for (int j = 0; j < _updateIndex.Count; j++)
                    {
                        var idx = _updateIndex[j];
                        list[idx]?.DoUpdate(dt);
                    }
                }
            }
        }

        public int CreateEntity()
        {
            // 扩容
            if (_emptyIndex.Count <= 0)
            {
                var size = _nowSize;
                _nowSize += _EXPAND_SIZE;
                for (int i = size; i < _nowSize; i++)
                {
                    _emptyIndex.Enqueue(i);
                }

                foreach (var comps in _typeToComponents.Values)
                {
                    for (int i = 0; i < _EXPAND_SIZE; i++)
                    {
                        comps.Add(null);
                    }
                }
            }

            var idx = _emptyIndex.Dequeue();
            _usingIndex.Add(idx);
            return idx;
        }

        public void DestroyEntity(int idx)
        {
            if (!_usingIndex.Remove(idx))
            {
                return;
            }
            foreach (var pair in _typeToComponents)
            {
                pair.Value[idx]?.Stop();
                pair.Value[idx] = null;

                if (_activeComponentIndex.TryGetValue(pair.Key, out var actives))
                {
                    actives.Remove(idx);
                }
            }
            _emptyIndex.Enqueue(idx);
        }

        public void AddComponent(int idx, BaseEntityComponent comp)
        {
            if (!_usingIndex.Contains(idx))
            {
                throw new Exception("Entity not created");
            }
            var type = comp.GetType();
            if (_typeToComponents.TryGetValue(type, out var list))
            {
                if (idx >= 0 && idx < list.Count)
                {
                    if (list[idx] != null)
                    {
                        throw new Exception("Duplicate add component");
                    }
                    if (!_activeComponentIndex.TryGetValue(type, out var activeIndex))
                    {
                        activeIndex = new();
                        _activeComponentIndex[type] = activeIndex;
                    }
                    list[idx] = comp;
                    list[idx]?.Start(idx);
                    activeIndex.Add(idx);
                }
            }
        }

        public void RemoveComponent<T>(int idx) where T : BaseEntityComponent
        {
            if (!_usingIndex.Contains(idx))
            {
                throw new Exception("Entity not created");
            }
            var type = typeof(T);
            if (!_typeToComponents.TryGetValue(type, out var list))
            {
                return;
            }
            var comp = list[idx];
            if (comp == null)
            {
                return;
            }
            comp.Stop();
            list[idx] = null;

            if (_activeComponentIndex.TryGetValue(type, out var activeIndex))
            {
                activeIndex.Remove(idx);
            }
        }
    }
}