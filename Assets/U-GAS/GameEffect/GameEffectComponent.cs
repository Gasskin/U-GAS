using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace U_GAS
{
    public class GameEffectComponent
    {
        public event Action OnGameEffectContainerDirty;
        private GameAbilityComponent _owner;
        private List<GameEffect.GameEffectSpec> _gameEffectSpecList = new();

        public void OnStart(GameAbilityComponent owner)
        {
            _owner = owner;
        }
        
        public void GetSpecList(List<GameEffect.GameEffectSpec> list)
        {
            list.Clear();
            list.AddRange(_gameEffectSpecList);
        }

        public void AddGameEffectSpec(GameEffect.GameEffectSpec spec)
        {
            if (!spec.CanApply())
            {
                UPool<GameEffect.GameEffectSpec>.Release(spec);
                return;
            }

            if (spec.IsImmune())
            {
                UPool<GameEffect.GameEffectSpec>.Release(spec);
                return;
            }
            
            spec.Start();

            if (spec.GameEffect.durationPolicy == EDurationPolicy.Instant)
            {
                spec.OnExecute();
                UPool<GameEffect.GameEffectSpec>.Release(spec);
                return;
            }

            switch (spec.GameEffect.stack.stackPolicy)
            {
                case EGameEffectStackPolicy.None:
                    break;
                case EGameEffectStackPolicy.Source:
                    break;
                case EGameEffectStackPolicy.Target:
                    break;
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