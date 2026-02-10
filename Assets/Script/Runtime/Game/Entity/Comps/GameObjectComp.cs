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

        private Vector3 _initPos;

        public GameObject View { get; private set; }

        public static GameObjectComp Get(string path, Transform parent, Vector3 initPos)
        {
            var comp = ObjectPool.Get<GameObjectComp>();
            comp._parent = parent;
            comp._assetPath = path;
            comp._initPos = initPos;
            return comp;
        }

        public override async UniTask Initialize()
        {
            View = await SystemDriver.YooSystem.InitializeGameObjectAsync(_assetPath, _parent);
            View.transform.position = _initPos;
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