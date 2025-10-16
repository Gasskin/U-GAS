using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace U_GAS
{
    public class GameEffectComponent
    {
        public event Action OnGameEffectContainerDirty;
        private GameAbilityComponent _owner;
        private List<GameEffectSpec> _gameEffectSpecList = new();

        public void OnStart(GameAbilityComponent owner)
        {
            _owner = owner;
        }
        
        public void GetSpecList(List<GameEffectSpec> list)
        {
            list.Clear();
            list.AddRange(_gameEffectSpecList);
        }

        public void AddGameEffectSpec(GameEffectSpec spec)
        {
            if (!spec.CanApply())
            {
                UPool<GameEffectSpec>.Release(spec);
                return;
            }

            if (spec.IsImmune())
            {
                UPool<GameEffectSpec>.Release(spec);
                return;
            }
            
            spec.Start();

            if (spec.GameEffect.durationPolicy == EDurationPolicy.Instant)
            {
                spec.OnExecute();
                UPool<GameEffectSpec>.Release(spec);
                return;
            }

            switch (spec.GameEffect.stack.stackPolicy)
            {
                case EGameEffectStackPolicy.None:
                {
                    AddNewGameEffectSpec();
                    break;
                }
                case EGameEffectStackPolicy.Target:
                {
                    if (spec.GameEffect.stack.hashKey == 0) 
                    {
                        AddNewGameEffectSpec();
                        return;
                    }
                    GameEffectSpec stackSpec = null;
                    foreach (var tSpec in _gameEffectSpecList)
                    {
                        if (tSpec.IsValid && tSpec.GameEffect.StackEqualTo(spec.GameEffect))
                        {
                            stackSpec = tSpec;
                        }
                    }
                    if (stackSpec == null) 
                    {
                        AddNewGameEffectSpec();
                        return;
                    }
                    
                    break;
                }
                case EGameEffectStackPolicy.Source:
                {
                    if (spec.GameEffect.stack.hashKey == 0)
                    {
                        AddNewGameEffectSpec();
                        return;
                    }
                    // todo
                    break;
                }
            }

            void AddNewGameEffectSpec()
            {
                _gameEffectSpecList.Add(spec);
                spec.OnAdd();
                if (spec.CanRunning())
                {
                    spec.OnActive();
                }
                
                OnGameEffectContainerDirty?.Invoke();   
            }
        }
    }
}