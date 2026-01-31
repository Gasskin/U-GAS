using System.Collections.Generic;
using cfg.Gas;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity.StateMachineComp.States;

namespace Script.Runtime.Game.Entity
{
    public class EntityHero
    {
        public int Level;
        public Dictionary<EAttributeId, float> InitAttributes;
        public int HeroId;

        public static async UniTask<Framework.System.Entity> Create(EntityHero heroInfo)
        {
            if (!SystemDriver.ConfigSystem.Tables.TbHero.DataMap.TryGetValue(heroInfo.HeroId, out var heroConfig))
            {
                return null;
            }

            FillAttributes(heroInfo);

            var e = SystemDriver.EntitySystem.CreateEntity();
            e.AddComp(new GasComp(heroInfo.Level, heroInfo.InitAttributes));
            e.AddComp(new GameObjectComp(heroConfig.PrefabPath, null));
            e.AddComp(new CampComp(ECamp.Player));
            e.AddComp(new BattleInputComp.BattleInputComp());

            var idle = new IdleState();
            var run = new RunState();
            var skillSpell = new SkillSpellState();
            e.AddComp(new StateMachineComp.StateMachineComp(idle, run, skillSpell));
            
            await e.Initialize();
            return e;
        }

        public static void FillAttributes(EntityHero heroInfo)
        {
            heroInfo.InitAttributes = new();
            // 基础值
            if (SystemDriver.ConfigSystem.Tables.TbLevelUp.DataMap.TryGetValue(heroInfo.Level, out var levelConfig))
            {
                heroInfo.InitAttributes.Add(EAttributeId.HpBase, levelConfig.BaseHp);
                heroInfo.InitAttributes.Add(EAttributeId.MpBase, levelConfig.BaseMp);
            }

            // 装备值
            // todo
            // 天赋值
            // todo
        }
    }
}