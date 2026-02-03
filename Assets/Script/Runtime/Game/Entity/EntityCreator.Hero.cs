using System;
using System.Collections.Generic;
using cfg.Gas;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using UnityEngine;

namespace Script.Runtime.Game.Entity
{
    public class EntityHero: IPoolObject
    {
    #region static
        public static async UniTask<Framework.System.Entity> Create(EntityHero heroInfo)
        {
            if (!SystemDriver.ConfigSystem.Tables.TbHero.DataMap.TryGetValue(heroInfo.HeroId, out var heroConfig))
            {
                return null;
            }

            FillAttributes(heroInfo);

            var e = SystemDriver.EntitySystem.CreateEntity();
            e.AddComp(GasComp.Get(heroInfo.Level, heroInfo._initAttributes));
            e.AddComp(GameObjectComp.Get(heroConfig.PrefabPath, null, Vector3.zero));
            e.AddComp(CampComp.Get(ECamp.Player));
            e.AddComp(ObjectPool.Get<BattleInputComp>());
            e.AddComp(ObjectPool.Get<HurtBodyComp>());

            var idle = new IdleState();
            var run = new RunState();
            var skillSpell = new SkillSpellState();
            e.AddComp(StateMachineComp.Get(idle, run, skillSpell));
            
            await e.Initialize();
            
            ObjectPool.Release(heroInfo);
            return e;
        }

        public static void FillAttributes(EntityHero heroInfo)
        {
            heroInfo._initAttributes = new();
            // 基础值
            if (SystemDriver.ConfigSystem.Tables.TbLevelUp.DataMap.TryGetValue(heroInfo.Level, out var levelConfig))
            {
                heroInfo._initAttributes.Add(EAttributeId.HpBase, levelConfig.BaseHp);
                heroInfo._initAttributes.Add(EAttributeId.MpBase, levelConfig.BaseMp);
            }

            // 装备值
            // todo
            // 天赋值
            // todo
        }
    #endregion
        
        public int Level;
        public int HeroId;
        
        private Dictionary<EAttributeId, float> _initAttributes;


        public void OnRelease()
        {
            Level = 0;
            HeroId = 0;
            _initAttributes = null;
        }
    }
}