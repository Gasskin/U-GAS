using UnityEngine.InputSystem;
    public partial class PlayerMap : BaseMap
    {
       public InputAction Move { get; private set; }

        public PlayerMap(InputActionMap inputMap) : base(inputMap)
        {
            Move = InputActionMap.FindAction("Move");
            RegisterAction(Move);
        }
        public override void Destroy()
        {
            UnRegisterAction(Move);
            base.Destroy();
        }
    }
