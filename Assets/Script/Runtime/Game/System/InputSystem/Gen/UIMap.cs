using UnityEngine.InputSystem;
    public partial class UIMap : BaseMap
    {

        public UIMap(InputActionAsset inputActionAsset, string inputMap) : base(inputActionAsset, inputMap)
        {
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
