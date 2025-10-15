using System;
using TMPro;
using U_GAS;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    public class Test : MonoBehaviour
    {
        public Image hp;
        public TextMeshProUGUI num;

        private int _entity;

        private GameAttribute attrHp;
        private GameAttribute attrMaxHp;

        private void Start()
        {
            _entity = EntitySystem.Instance.CreateEntity();
            _entity.AddComponent(new GameAbilityComponent());

            if (_entity.TryGetComponent(out GameAbilityComponent component))
            {
                attrHp = new GameAttribute(EGameAttribute.Hp, ECalculateMode.Stacking, 200, 0, int.MaxValue);
                attrMaxHp = new GameAttribute(EGameAttribute.Strength, ECalculateMode.Stacking, 200, 0, int.MaxValue);
                // attrMaxHp.SetDepend(attrStrength, null);

                component.GameAttributeComponent.AddAttribute(attrHp);
                component.GameAttributeComponent.AddAttribute(attrMaxHp);
                num.text = $"{attrHp.CurrentValue}/{attrMaxHp.CurrentValue}";
                hp.fillAmount = attrHp.CurrentValue / attrMaxHp.CurrentValue;

                attrHp.OnPostCurrentValueChange += OnPostHpCurrentValueChange;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_entity.TryGetComponent(out GameAbilityComponent owner))
                {
                    owner.ApplyGameEffectTo(CommonDamageEffectTemplate.Get(), owner);
                }
            }
        }

        private void OnPostHpCurrentValueChange(GameAttribute arg1, float arg2, float arg3)
        {
            num.text = $"{attrHp.CurrentValue}/{attrMaxHp.CurrentValue}";
            hp.fillAmount = attrHp.CurrentValue / attrMaxHp.CurrentValue;
        }
    }
}