using Cysharp.Threading.Tasks;
using Script.Runtime.Framework.System;
using Script.Runtime.Game.Entity;
using UnityEngine;

namespace Script.Runtime.Game.UI.EnemyHud
{
    public class EnemyHudWindow : BaseWindow
    {
        public static UIConfig Config = new UIConfig()
        {
            Layer = EUILayer.EnemyHud,
            Path = "Assets/Bundles/UI/EnemyHud/EnemyHudWindow.prefab",
            FullScreen = false,
        };

        public Transform Root;

        protected override void OnTick(float dt)
        {
        }

        protected override void OnCreate()
        {
            EventGroup.AddListener<OnEntityCreateEvent>(OnEntityCreate);
        }

        protected override void OnDispose()
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        private void OnEntityCreate(BaseEventMessage obj)
        {
            if (obj is not OnEntityCreateEvent e)
            {
                return;
            }
            if (SystemDriver.EntitySystem.HasComp(e.EntityId, out CampComp campComp))
            {
                if (campComp.Camp != ECamp.Player)
                {
                    OnEntityCreateAsync(e.EntityId).Forget();
                }
            }
        }

        private async UniTaskVoid OnEntityCreateAsync(ulong entityId)
        {
            var widget = (EnemyStateBarWidget)await AddDynamicWidget(EnemyStateBarWidget.PrefabPath, Root);
            if (!IsValid || widget == null)
            {
                return;
            }
            widget.SetEntity(entityId);
        }
    }
}