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

        private int entity;
        
        private void Start()
        {
            entity = EntitySystem.Instance.CreateEntity();
            entity.AddComponent(new GameAbilityComponent());

            if (entity.TryGetComponent(out GameAbilityComponent component))
            {
                var attrHp = new GameAttribute(100, 0, 100);
                var attrStrength = new GameAttribute(10, 0, int.MaxValue);
                var attrMaxHp = new GameAttribute(1, 0, int.MaxValue);
                // attrMaxHp.SetDepend(attrStrength, null);
                
                component.GameAttributeComponent.AddAttribute(EGameAttribute.Hp, attrHp);
                component.GameAttributeComponent.AddAttribute(EGameAttribute.MaxHp, attrMaxHp);
                component.GameAttributeComponent.AddAttribute(EGameAttribute.Strength, attrStrength);
                
                attrHp = component.GameAttributeComponent.GetAttribute(EGameAttribute.Hp);
            }
        }
    }
}