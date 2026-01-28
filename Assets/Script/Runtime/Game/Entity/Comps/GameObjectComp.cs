using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using UnityEngine;

namespace Script.Runtime.Game.Entity
{
    [EntityCompPriority(Priority_View)]
    public class GameObjectComp : EntityComp
    {

        private string _assetPath;

        private Transform _parent;

        public GameObject View { get; private set; }

        public GameObjectComp(string path, Transform parent)
        {
            _assetPath = path;
            _parent = parent;
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