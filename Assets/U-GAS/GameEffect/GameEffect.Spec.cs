using System;
using System.Collections.Generic;

namespace U_GAS
{
    public partial class GameEffect
    {
        public GameEffectSpec CreateSpec(GameAbilityComponent source, GameAbilityComponent target)
        {
            var spec = UPool<GameEffectSpec>.Get();
            spec.Init(this, source, target);
            return spec;
        }

        public class GameEffectSpec : IUPoolObject
        {
            private static ulong _idFactory;
            public bool IsValid => Id > 0;
            public GameEffect GameEffect { get; private set; }
            public bool IsActive { get; private set; }

            public ulong Id { get; private set; }

            public GameAbilityComponent Source { get; private set; }

            public GameAbilityComponent Target { get; private set; }

            public Dictionary<EGameAttribute, float> SourceSnap { get; private set; } = new();
            public Dictionary<EGameAttribute, float> TargetSnap { get; private set; } = new();

            public float ActiveTime { get; private set; }

            public void Init(GameEffect gameEffect, GameAbilityComponent source, GameAbilityComponent target)
            {
                GameEffect = gameEffect;
                IsActive = false;
                Id = ++_idFactory;
                Source = source;
                Target = target;
                SourceSnap.Clear();
                TargetSnap.Clear();
                ActiveTime = 0;
            }

            public void OnRelease()
            {
                GameEffect = null;
                IsActive = true;
                Id = 0;
                Source = null;
                Target = null;
                SourceSnap.Clear();
                TargetSnap.Clear();
                ActiveTime = 0;
            }

            public void Start()
            {
                // period ge
                SnapShot();
            }

            public bool CanApply()
            {
                if (GameEffect.applyRequiredTags == null)
                {
                    return true;
                }
                return Target.GameTagComponent.HasAllTag(GameEffect.applyRequiredTags);
            }

            public bool CanRunning()
            {
                if (GameEffect.ongoingRequiredTags == null)
                {
                    return true;
                }
                return Target.GameTagComponent.HasAllTag(GameEffect.ongoingRequiredTags);
            }

            public bool IsImmune()
            {
                if (GameEffect.immuneTags == null)
                {
                    return false;
                }
                return Target.GameTagComponent.HasAnyTags(GameEffect.immuneTags);
            }

            private void SnapShot()
            {
                if (!GameEffect.needSnapShot)
                {
                    return;
                }
                Source.GameAttributeComponent.DoSnapShot(SourceSnap);
                Target.GameAttributeComponent.DoSnapShot(TargetSnap);
            }

            public void OnExecute()
            {
                // On Cue Execute
                Target.ApplyModFromInstantGameEffect(this);
                if (IsValid)
                {
                    // period ge可能会移除父ge，此时自身也已经失效了
                }
            }

            public void OnAdd()
            {
                // On Cue Add
            }

            public void OnRemove()
            {
            }

            public void OnActive()
            {
                if (IsActive)
                {
                    return;
                }
                IsActive = true;
                ActiveTime = UTime.CurrentTime;

          
                Target.GameTagComponent.AddTagsWithDirty(GameEffect.grantedTags);
                // todo remove effect

                // AddTag可能会触发TagDirty，可能会导致GE被删除
                if (IsValid)
                {
                    // TryActivateGrantedAbilities();
                    // ability可能导致自身失效
                    if (IsValid)
                    {
                        // TriggerCueOnActivation();
                    }
                }
            }


            public void OnDeActive()
            {
                if (!IsActive)
                {
                    return;
                }
                IsActive = false;
                
                Target.GameTagComponent.RemoveTagsWithDirty(GameEffect.grantedTags);

                if (IsValid)
                {
                    // TryDeactivateGrantedAbilities();
                    // ability可能导致自身失效
                    if (IsValid)
                    {
                        // TriggerCueOnDeactivation();
                    }
                }
            }
        }
    }
}