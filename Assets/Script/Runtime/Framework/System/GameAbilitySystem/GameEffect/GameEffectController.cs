using System.Collections.Generic;
using cfg.Gas;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Game;
using Script.Runtime.Game.System.GameAbilitySystem;
using UnityEngine.Pool;

namespace Script.Runtime.Framework.System.GameEffect
{
    public class GameEffectController
    {
        private GasComp _owner;
        private readonly List<GameEffectSpec> _gameEffectSpecs = new();
        private List<GameEffectSpec> _tickPool = new(128);
        public void Init(GasComp owner)
        {
            _owner = owner;
        }

        public void Tick(float dt)
        {
            _tickPool.Clear();
            _tickPool.AddRange(_gameEffectSpecs);
            for (int i = 0; i < _tickPool.Count; i++)
            {
                var spec = _tickPool[i];
                if (spec.IsActive)
                {
                    spec.OnTick(dt);
                }
            }
        }

        public void OnTagDirty()
        {
            foreach (var spec in _gameEffectSpecs)
            {
                if (spec.IsActive)
                {
                    if (!spec.CanRunning())
                    {
                        spec.OnDeActive();
                    }
                }
                else
                {
                    if (spec.CanRunning())
                    {
                        spec.OnActive();
                    }
                }
            }
        }

        public ulong AddGameEffectSpec(GameEffectSpec spec)
        {
            if (!spec.CanApply())
            {
                ObjectPool.ObjectPool.Release(spec);
                return 0;
            }

            if (spec.IsImmune())
            {
                ObjectPool.ObjectPool.Release(spec);
                return 0;
            }

            // 瞬时GE
            if (spec.GameEffect.Duration.DurationType == EGameEffectDurationType.Instant)
            {
                spec.OnExecute();
                ObjectPool.ObjectPool.Release(spec);
                return 0;
            }

            // 不堆叠
            if (spec.GameEffect.Stack.StackType == EGameEffectStackType.None)
            {
                if (AddNewGameEffectSpec(spec.GameEffect.Stack.CountLimit))
                {
                    return spec.Id;
                }
                ObjectPool.ObjectPool.Release(spec);
                return 0;
            }

            GameEffectSpec stackSpec = null;
            for (int i = 0; i < _gameEffectSpecs.Count; i++)
            {
                var tSpec = _gameEffectSpecs[i];
                // 按照来源堆叠，要求ge的来源是同一个人
                // 否则按照目标堆叠，只要ge相同就堆起来就行
                if (spec.GameEffect.Stack.StackType == EGameEffectStackType.Source && tSpec.Source != spec.Source)
                {
                    continue;
                }
                if (tSpec.IsValid && tSpec.GameEffect.Id == spec.GameEffect.Id)
                {
                    stackSpec = tSpec;
                    break;
                }
            }
            // 不存在，直接新增
            if (stackSpec == null)
            {
                AddNewGameEffectSpec(0);
                return spec.Id;
            }
            stackSpec.AddStack(1, spec.GameEffect.Stack.RefreshDurationOnStack, spec.GameEffect.Stack.RefreshPeriodOnStack);
            // 堆叠后可以直接释放
            ObjectPool.ObjectPool.Release(spec);

            return stackSpec.Id;

            bool AddNewGameEffectSpec(int countLimit)
            {
                // 总数检查
                if (countLimit > 0)
                {
                    var has = 0;
                    for (int i = 0; i < _gameEffectSpecs.Count; i++)
                    {
                        if (_gameEffectSpecs[i].GameEffect.Id == spec.GameEffect.Id)
                        {
                            has++;
                            if (has >= countLimit)
                            {
                                return false;
                            }
                        }
                    }
                }
                _gameEffectSpecs.Add(spec);
                spec.OnAdd();
                if (spec.CanRunning())
                {
                    spec.OnActive();
                }
                return true;
            }
        }

        public void GetGameEffectSpecs(List<GameEffectSpec> effects)
        {
            effects.Clear();
            effects.AddRange(_gameEffectSpecs);
        }

        public void RemoveGameEffectWithAnyTags(List<EGameTag> withTags)
        {
            var list = ListPool<GameEffectSpec>.Get();
            foreach (var spec in _gameEffectSpecs)
            {
                foreach (var tag in withTags)
                {
                    if (spec.GameEffect.AssetTagContainer.HasTag(tag))
                    {
                        list.Add(spec);
                    }
                    else if (spec.GameEffect.GrantedTagContainer.HasTag(tag))
                    {
                        list.Add(spec);
                    }
                }
            }
            foreach (var spec in list)
            {
                InternalRemoveGameEffectSpec(spec);
            }

            ListPool<GameEffectSpec>.Release(list);
        }

        public void InternalRemoveGameEffectSpec(GameEffectSpec spec)
        {
            // 必须先删除，DeActive可能触发TagDirty
            _gameEffectSpecs.Remove(spec);
            spec.OnDeActive();
            spec.OnRemove();
            ObjectPool.ObjectPool.Release(spec);
        }
    }
}