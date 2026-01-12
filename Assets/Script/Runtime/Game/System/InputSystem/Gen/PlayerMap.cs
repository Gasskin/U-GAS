using UnityEngine.InputSystem;
    public partial class PlayerMap : BaseMap
    {
       public InputAction Move { get; private set; }

        public PlayerMap(InputActionAsset inputActionAsset, string inputMap) : base(inputActionAsset, inputMap)
        {
            Move = InputActionMap.FindAction("Move");
            RegisterAction(Move);
        }
        public override void Dispose()
        {
            UnRegisterAction(Move);
            base.Dispose();
        }
    }
