using cfg.Gas;
using Script.Runtime.Framework;
using Script.Runtime.Framework.System;
using Script.Runtime.Framework.System.GameAttribute;
using Script.Runtime.Game.Entity;
using TMPro;
using UnityEngine.UI;

namespace Script.Runtime.Game.UI.BattleHud.Widget
{
    public class HpBarWidget : BaseWidget
    {
        public EAttributeId Now;
        public EAttributeId Max;

        public TextMeshProUGUI Num;
        public Image Fill;

        private ulong _entityId;
        private GameAttribute _now;
        private GameAttribute _max;

        public override void OnTick(float dt)
        {
        }

        public override void OnOpen()
        {
            gameObject.SetActive(false);
            if (SystemDriver.PlayerDataSystem.Player != null)
            {
                SetEntity(SystemDriver.PlayerDataSystem.Player.Id);
            }
            else
            {
                ParentWindow.EventGroup.AddListener<OnEntityCreateEvent>(OnEntityCreate);
            }
        }

        public override void OnClose()
        {
            if (_now != null)
            {
                _now.OnPostCurrentValueChange -= OnValueChange;
            }
            if (_max != null)
            {
                _max.OnPostCurrentValueChange -= OnValueChange;
            }
        }


        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        private void OnEntityCreate(BaseEventMessage obj)
        {
            if (obj is not OnEntityCreateEvent e)
            {
                return;
            }
            if (SystemDriver.EntitySystem.HasComp(e.EntityId, out CampComp camp) && camp.Camp == ECamp.Player)
            {
                SetEntity(e.EntityId);
            }
        }

        private void SetEntity(ulong entityId)
        {
            gameObject.SetActive(true);
            _entityId = entityId;
            SystemDriver.EntitySystem.HasComp(_entityId, out GasComp gasComp);
            _now = gasComp.GameAttributeController.GetAttribute(Now);
            _max = gasComp.GameAttributeController.GetAttribute(Max);
            _now.OnPostCurrentValueChange += OnValueChange;
            _max.OnPostCurrentValueChange += OnValueChange;

            OnValueChange(_now.CurrentValue, _now.CurrentValue);
        }

        private void OnValueChange(float a1, float a2)
        {
            var now = _now.CurrentValue;
            var max = _max.CurrentValue;
            if (max <= 0) 
            {
                Fill.fillAmount = 0;
                Num.text = "0/0";
                return;
            }
            Fill.fillAmount = now / max;
            Num.text = $"{(int)now}/{(int)max}";
        }
    }
}