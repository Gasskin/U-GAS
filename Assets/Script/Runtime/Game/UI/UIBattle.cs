using cfg.Gas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIBattle : MonoBehaviour
    {
        public Image HpBar;
        public TextMeshProUGUI HpText;
        public Image MpBar;
        public TextMeshProUGUI MpText;

        private Entity _main;

        private GameAttribute _hpNow;
        private GameAttribute _hpMax;
        private GameAttribute _mpNow;
        private GameAttribute _mpMax;

        void Start()
        {
        }

        void Update()
        {
        }


        public void SetRole(Entity main)
        {
            _main = main;

            _main.HasComp<GasComp>(EntityComp.Priority_Gas, out var gas);
            _hpNow = gas.GameAttributeController.GetAttribute(EAttributeId.HpNow);
            _hpMax = gas.GameAttributeController.GetAttribute(EAttributeId.HpMax);
            _mpNow = gas.GameAttributeController.GetAttribute(EAttributeId.MpNow);
            _mpMax = gas.GameAttributeController.GetAttribute(EAttributeId.MpMax);

            _hpNow.OnPostCurrentValueChange += OnHpPoseCurrentValueChange;
            _mpNow.OnPostCurrentValueChange += OnMpPoseCurrentValueChange;
            
            OnHpPoseCurrentValueChange(_hpNow.CurrentValue,_hpNow.CurrentValue);
            OnMpPoseCurrentValueChange(_mpNow.CurrentValue,_mpNow.CurrentValue);
        }

        private void OnHpPoseCurrentValueChange(float prev, float now)
        {
            HpBar.fillAmount = now / _hpMax.CurrentValue;
            HpText.text = $"{(int)now}/{(int)_hpMax.CurrentValue}";
        }

        private void OnMpPoseCurrentValueChange(float prev, float now)
        {
            MpBar.fillAmount = now / _mpMax.CurrentValue;
            MpText.text = $"{(int)now}/{(int)_mpMax.CurrentValue}";
        }
    }
}