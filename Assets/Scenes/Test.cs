using U_GAS;
using UnityEngine;

namespace Scenes
{
    public class Test : MonoBehaviour
    {
        public EGameTag tag;

        private int _entity;
        
        private void Start()
        {
            _entity = EntitySystem.Instance.CreateEntity();
            _entity.AddComponent(new GameAbilityComponent());

            if (_entity.TryGetComponent(out GameAbilityComponent component))
            {
                component.GameAttributeComponent.AddAttributeSet(EGameAttributeSet.Number);
                component.GameAttributeComponent.InitAttr(EGameAttribute.Hp, EGameAttributeSet.Number, 1000);

                var value = component.GameAttributeComponent.GetAttrBaseValue(EGameAttribute.Hp, EGameAttributeSet.Number);
                Debug.LogError(value);
            }
            Debug.LogError(111);
        }
    }
}