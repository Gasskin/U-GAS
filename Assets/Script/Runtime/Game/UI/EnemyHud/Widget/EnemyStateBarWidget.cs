using cfg.Gas;
using Script.Runtime.Framework.System;
using Script.Runtime.Framework.System.GameAttribute;
using Script.Runtime.Game.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Runtime.Game.UI.EnemyHud
{
    public class EnemyStateBarWidget : BaseWidget
    {
        public const string PrefabPath = "Assets/Bundles/UI/EnemyHud/Widget/EnemyStateBarWidget.prefab";

        public Image HpImage;
        public TextMeshProUGUI HpText;

        private Framework.System.Entity _entity;
        private GameObjectComp _gameObjectComp;
        private GameAttribute _nowHp;
        private GameAttribute _maxHp;
        
        private Camera _uiCamera;
        private RectTransform _parentRect;
        
        public override void OnTick(float dt)
        {
            if (_gameObjectComp == null) 
            {
                return;
            }
            var screenPoint = Camera.main.WorldToScreenPoint(_gameObjectComp.View.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRect, screenPoint, _uiCamera,
                out var localPoint);
            transform.localPosition = localPoint;
        }

        protected override void OnCreate()
        {
            _uiCamera = SystemDriver.Instance.UIRoot.UICamera;
            _parentRect = ((EnemyHudWindow)ParentWindow).Root as RectTransform;
        }

        protected override void OnDispose()
        {
            _entity = null;
            if (_maxHp != null) 
            {
                _maxHp.OnPostCurrentValueChange -= OnValueChange;
            }
            if (_nowHp != null) 
            {
                _nowHp.OnPostCurrentValueChange -= OnValueChange;
            }
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        public void SetEntity(ulong entityId)
        {
            SystemDriver.EntitySystem.HasEntity(entityId, out _entity);
            if (_entity != null)
            {
                EventGroup.AddListener<OnEntityDestroyEvent>(OnEntityDestroyEvent);

                SystemDriver.EntitySystem.HasComp(entityId, out _gameObjectComp);
                SystemDriver.EntitySystem.HasComp(entityId, out GasComp gasComp);
                
                _nowHp = gasComp.GameAttributeController.GetAttribute(EAttributeId.HpNow);
                _maxHp = gasComp.GameAttributeController.GetAttribute(EAttributeId.HpMax);
                _nowHp.OnPostCurrentValueChange += OnValueChange;
                _maxHp.OnPostCurrentValueChange += OnValueChange;
                OnValueChange(0,0);
            }
        }


        private void OnEntityDestroyEvent(BaseEventMessage obj)
        {
            if (obj is not OnEntityDestroyEvent e)
            {
                return;
            }
            if (_entity != null && _entity.Id == e.EntityId)
            {
                ParentWindow.RemoveDynamicWidget(this);
            }
        }
        
        private void OnValueChange(float pre, float now)
        {
            var nowHp = _nowHp.CurrentValue;
            var maxHp = _maxHp.CurrentValue;
            if (maxHp <= 0) 
            {
                HpImage.fillAmount = 0;
                HpText.text = "0/0";
                return;
            }
            HpImage.fillAmount = nowHp / maxHp;
            HpText.text = $"{(int)nowHp}/{(int)maxHp}";
        }
    }
}