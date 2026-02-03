using System;
using System.Collections.Generic;
using cfg.Gas;
using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using UnityEngine;

namespace Script.Runtime.Game.Entity
{
    public class EntityMonster: IPoolObject
    {
    #region static
        public static async UniTask<Framework.System.Entity> Create(EntityMonster monsterInfo)
        {
            if (!SystemDriver.ConfigSystem.Tables.TbMonster.DataMap.TryGetValue(monsterInfo.MonsterId, out var monsterConfig))
            {
                return null;
            }
            
            FillAttributes(monsterInfo);

            var e = SystemDriver.EntitySystem.CreateEntity();
            e.AddComp(GasComp.Get(monsterInfo.Level, monsterInfo._initAttributes));
            e.AddComp(GameObjectComp.Get(monsterConfig.PrefabPath, null, Vector3.zero));
            e.AddComp(CampComp.Get(ECamp.Monster));
            e.AddComp(ObjectPool.Get<HurtBodyComp>());
            
            await e.Initialize();
            return e;
        }

        public static void FillAttributes(EntityMonster monsterInfo)
        {
            monsterInfo._initAttributes = new();

            monsterInfo._initAttributes.Add(EAttributeId.HpBase, 10000);
        }
    #endregion
        
        public int Level;
        public int MonsterId;

        private Dictionary<EAttributeId, float> _initAttributes;


        public void OnRelease()
        {
            Level = 0;
            MonsterId = 0;
            _initAttributes = null;
        }
    }
}