using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;
using UnityEngine;

namespace Script.Runtime.Game.Entity
{
    [EntityCompPriority(Priority_GameObject)]
    public class GameObjectComp : EntityComp
    {

        private string _assetPath;

        private Transform _parent;

        public GameObject View { get; private set; }

        public static GameObjectComp Get(string path, Transform parent)
        {
            var comp = ObjectPool.Get<GameObjectComp>();
            comp._parent = parent;
            comp._assetPath = path;
            return comp;
        }

        public override async UniTask Initialize()
        {
            View = await SystemDriver.YooSystem.InitializeGameObjectAsync(_parent, _assetPath);
            if (!IsValid)
            {
                Object.Destroy(View);
            }
        }

        public override void Destroy()
        {
            if (View != null) 
            {
                Object.Destroy(View);
            }
        }
    }
}